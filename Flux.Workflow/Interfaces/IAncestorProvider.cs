using System;
using System.Collections.Generic;

namespace Flux.Workflow.Interfaces
{
    public interface IAncestorProvider
    {
        IEnumerable<Object> Ancestors { get; }
    }
}
