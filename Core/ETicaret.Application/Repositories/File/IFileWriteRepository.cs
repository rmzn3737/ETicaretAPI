﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaretAPI.Domain.Entities;

namespace ETicaret.Application.Repositories
{
    public interface IFileWriteRepository:IWriteRepository<ETicaretAPI.Domain.Entities.File>
    {
    }
}
