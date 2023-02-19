//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace MD5Namespace;

// **************************************************************
// * Raw implementation of the MD5 hash algorithm
// * from RFC 1321.
// *
// * Written By: Reid Borsuk and Jenny Zheng
// * Copyright (c) Microsoft Corporation.  All rights reserved.
// **************************************************************

// Simple struct for the (a,b,c,d) which is used to compute the mesage digest.    
internal struct AbcdStruct
{
    public uint A;
    public uint B;
    public uint C;
    public uint D;
}
