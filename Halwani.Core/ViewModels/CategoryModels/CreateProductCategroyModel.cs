﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.CategoryModels
{
   public class CreateProductCategroyModel
    {
        public string ParentCategory { get; set;}
        public List<SubCateogryModel> SubCategory { get; set; }
    }
}