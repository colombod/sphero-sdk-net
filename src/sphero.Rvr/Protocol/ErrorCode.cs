﻿namespace sphero.Rvr.Protocol;

public enum ErrorCode : byte
{
    success = 0x00,
    bad_did = 0x01,
    bad_cid = 0x02,
    not_yet_implemented = 0x03,
    restricted = 0x04,
    bad_data_length = 0x05,
    failed = 0x06,
    bad_parameter_value = 0x07, // At least one data parameter is invalid
    busy = 0x08,
    bad_tid = 0x09,
    target_unavailable = 0x0A
}