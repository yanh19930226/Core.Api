﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Entity.ViewModels
{
    /// <summary>
    ///     验证错误信息视图模型
    /// </summary>
    public class ErrorView
    {
        /// <summary>
        ///     错误字段
        /// </summary>

        public string ErrorName { get; set; }

        /// <summary>
        ///     错误内容ErrorMessage
        /// </summary>

        public string Error { get; set; }
    }
}
