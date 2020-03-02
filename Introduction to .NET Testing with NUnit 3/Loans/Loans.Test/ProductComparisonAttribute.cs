using System;
using NUnit.Framework;

namespace Loans.Test
{    
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class ProductComparisonAttribute : CategoryAttribute
    {
        
    }
}