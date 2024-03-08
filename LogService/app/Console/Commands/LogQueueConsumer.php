<?php

namespace App\Console\Commands;

use App\Services\LogQueueEventHandler;
use Illuminate\Console\Command;
use Illuminate\Support\Facades\Log;
use PhpAmqpLib\Connection\AMQPStreamConnection;
use PhpParser\JsonDecoder;

class LogQueueConsumer extends Command
{


    /**
     * The name and signature of the console command.
     *
     * @var string
     */
    protected $signature = 'rabbitmq:consumer';

    /**
     * The console command description.
     *
     * @var string
     */
    protected $description = 'RabbitMQ Consumer';
    /**
     * Execute the console command.
     *
     * @return int
     */


    /**
     * @var LogQueueEventHandler
     */
    protected $logQueueEventHandler;

    /**
     * Create a new command instance.
     *
     * @return void
     */
    public function __construct(LogQueueEventHandler $logQueueEventHandler)
    {
        $this->logQueueEventHandler = $logQueueEventHandler;
        parent::__construct();
    }


    /**
     * queue_declare function:
     * Declares queue, creates if needed
     *
     * @param string $queue
     * @param bool $passive
     * @param bool $durable
     * @param bool $exclusive
     * @param bool $auto_delete
     * @param bool $nowait
     * @param array|AMQPTable $arguments
     * @param int|null $ticket
     * @return array|null
     * @throws \PhpAmqpLib\Exception\AMQPTimeoutException if the specified operation timeout was exceeded
     */
    /**
     * basic_consume function:
     * @param string consumer_tag: Consumer identifier
     * @param bool no_local: Don't receive messages published by this consumer.
     * @param bool no_ack: If set to true, automatic acknowledgement mode will be used by this consumer. See https://www.rabbitmq.com/confirms.html for details.
     * @param bool exclusive: Request exclusive consumer access, meaning only this consumer can access the queue
     * @param bool nowait:
     * callback: A PHP Callback
     */
    public function handle()
    {
        $connection = new AMQPStreamConnection('localhost', 5672, 'guest', 'guest');
        $channel = $connection->channel();
        $channel->queue_declare(
            'asyncBusLog',
            false,
            true,
            false,
            false
        );
        echo " --> Waiting for messages. To exit press CTRL+C\n";
        $callback = function ($msg) {
            echo ' --> Received ', $msg->body, "\n";
            $msg->delivery_info['channel']->basic_ack($msg->delivery_info['delivery_tag']);
            $decodedMsg = json_decode($msg->body);
            $this->logQueueEventHandler->handle($decodedMsg);
        };
        $channel->basic_qos(null, 1, null);
        $channel->basic_consume(
            'asyncBusLog',
            '',
            false,
            false,
            false,
            false,
            $callback
        );
        while ($channel->is_consuming()) {
            $channel->wait();
        }
        Log::info("[X] --> Closing AsyncQueueLog Consumer");
        $channel->close();
        return Command::SUCCESS;
    }
}
