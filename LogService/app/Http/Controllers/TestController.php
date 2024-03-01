<?php

namespace App\Http\Controllers;

use App\Jobs\TestQueue;
use Exception;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Log;
use PHPUnit\Event\Code\Test;

class TestController extends Controller
{
    public function test(Request $request)
    {
        $data = ['name' => 'Jon Doe', 'phone' => '12345678901'];
        dispatch(new TestQueue($data));

        
        return response()->json(['message' => 'Job dispatched']);
    }
}
