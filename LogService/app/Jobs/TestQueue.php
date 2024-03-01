<?php

namespace App\Jobs;

use App\Models\UserInfo;
use Illuminate\Bus\Queueable;
use Illuminate\Contracts\Queue\ShouldBeUnique;
use Illuminate\Contracts\Queue\ShouldQueue;
use Illuminate\Foundation\Bus\Dispatchable;
use Illuminate\Queue\InteractsWithQueue;
use Illuminate\Queue\SerializesModels;
use Illuminate\Support\Facades\Log;

class TestQueue implements ShouldQueue
{
    private $data;

    /**
     * Create a new job instance.
     *
     * @return void
     */
    public function __construct($data)
    {
        $this->data = $data;
        Log::info("--> MessageBus connected");
    }

    /**
     * Execute the job.
     *
     * @return void
     */
    public function handle()
    {
        Log::error("--> Test queue recieved a request");
    }
}
