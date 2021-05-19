using Halwani.Core.ViewModels.GenericModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ModelRepositories.Interfaces
{
  public interface IlocationRepository
    {
        IEnumerable<LookupViewModel> GetLocations();
    }
}
