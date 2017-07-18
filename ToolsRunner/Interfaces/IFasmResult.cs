namespace ToolsRunner.Interfaces
{
    public interface IFasmResult
    {
        string ErrorResult { get;  }
        string Output { get;  }
    }
}