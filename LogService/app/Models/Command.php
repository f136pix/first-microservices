<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Command extends Model
{
    protected $fillable = [
        'howTo',
        'commandLine',
        'platform_id',
    ];

    public function platform()
    {
        return $this->belongsTo(Platform::class);
    }
}
