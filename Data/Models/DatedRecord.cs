﻿using System;

namespace Data.Models;

public class DatedRecord
{
    public DateTimeOffset CreatedDateTime { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedDateTime { get; set; }
}