<?php

namespace App\Services;

use App\Models\Command;
use App\Models\Platform;
use Illuminate\Support\Facades\Log;

class LogQueueEventHandler
{
    public function handle($data)
    {
        echo "Handling message \n";
        switch ($data->Event) {
            case "Platform_Published": // adding platform
                echo "--> Creating Plat <--";
                try {
                    $plat = new Platform;
                    $plat->name = $data->Name;
                    $plat->save();
                    echo "--> Platform created Succefully \n";
                    echo "$plat";
                } catch (\Exception $e) {
                    echo "--> Issue creating the Platform : $e";
                }
                break;
            case "Command_Published": // adding platform
                echo "-->Creating a command <--";
                try {
                    $comm = new Command;
                    $comm->howTo = $data->HowTo;
                    $comm->commandLine = $data->CommandLine;
                    $comm->platform_id = $data->PlatformId;
                    $comm->save();
                    echo "--> Command created Succefully \n";
                    echo "$comm";
                } catch (\Exception $e) {
                    echo "--> Issue creating the Command : $e";
                }

                break;
            default:
                Log::info('Could not define the type of the Event');
                $eventValue = $data->Event ?? "Event property not found";
                echo "Could not define the type of the Event \n Data : $eventValue";
                break;
        }
    }
}
