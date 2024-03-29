﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Dashboard.BLOB.Dto;
using Volo.Abp.Application.Services;

namespace Dashboard.BLOB
{
    public interface IBlobService : IBlobBaseService
    {
        Task<List<GetBlobsDto>> GetBackGroundListAsync(string name);
    }
}