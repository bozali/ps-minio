﻿using Minio.DataModel.Args;

using System.Management.Automation;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Bal.PsMinio.Management.Commands.Buckets;

[Cmdlet(VerbsDiagnostic.Test, "MinioBucket")]
public class TestMinioBucketCommand : ClientOperation
{
    [ValidateNotNull]
    [Parameter(Mandatory = true, ValueFromPipeline = true)]
    public string BucketName { get; set; }

    protected override void ProcessRecord()
    {
        this.WriteObject(this.Client.Context
            .BucketExistsAsync(new BucketExistsArgs().WithBucket(this.BucketName))
            .GetAwaiter()
            .GetResult());
    }
}