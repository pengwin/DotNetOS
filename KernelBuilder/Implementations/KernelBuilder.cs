using System;
using System.Threading.Tasks;
using Common;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.Factories;
using KernelBuilder.Interfaces;
using KernelBuilder.KernelWrapper;
using ToolsRunner.Interfaces;

namespace KernelBuilder.Implementations
{
    internal class KernelBuilder : IKernelBuilder
    {
        private const int BaseAddress = 0x00100000;

        private const string Bss = "__bss";

        private const string BssEnd = "__bss_end";

        private const string End = "__end";

        private const string Stack = "__stack_start";

        private const string MultibootHeaderLabel = "__multiboot_header";

        private const int StackSizeInBytes = 1024*1024; // 1MB

        private readonly ISourceBuilder _sources;

        private readonly ICodeContextFactory _codeContextFactory;

        private readonly IFasmRunner _fasmRunner;

        public KernelBuilder(ISourceBuilder sources, ICodeContextFactory codeContextFactory, IFasmRunner fasmRunner)
        {
            _sources = sources;
            _codeContextFactory = codeContextFactory;
            _fasmRunner = fasmRunner;
        }

        public async Task<IKernel> BuildAsync(Type kernelType, bool useTextMode = true)
        {
            return await BuildAsync(kernelType, Option<Type>.None, useTextMode);
        }

        public async Task<IKernel> BuildAsync(Type kernelType, Option<Type> runtime, bool useTextMode = true)
        {
            
            var sources = BuildSources(kernelType, runtime, useTextMode);
            var kernel = new Kernel(sources);
            var compilationResult = await Compile(kernel);
            if (!string.IsNullOrEmpty(compilationResult.ErrorResult))
            {
                throw new Exception(compilationResult.ErrorResult);
            }
            return kernel;
        }

        private string BuildSources(Type kernelType, Option<Type> runtime, bool useTextMode)
        {
            var runtimeDefinition = runtime.IsNone
                ? Option<IRuntimeDefinition>.None
                : Option<IRuntimeDefinition>.New(new RuntimeDefinition(runtime.Value));
            var context = _codeContextFactory.GetCodeContext(runtimeDefinition);
            var sources = context.ResolveType(kernelType);
            if (sources.IsNone)
            {
                throw new ArgumentException($"Could not get type body for type {kernelType}", nameof(kernelType));
            }
            var entryChunk = new EntryChunk(sources.Value,Stack, context.DefaultConstructorsLabel);

            var header = new MultibootHeaderChunk(new MultibootHeaderInfo
            {
                Flags = MultibootHeaderInfo.HeaderFlags.PageAlign | MultibootHeaderInfo.HeaderFlags.UseMultibootAddressFields | MultibootHeaderInfo.HeaderFlags.VideoMode,
                AddressFields = new MultibootAddressFields
                {
                    HeaderLabel = MultibootHeaderLabel,
                    LoadStartLabel = MultibootHeaderLabel,
                    LoadEndLabel = Bss,
                    BssEndLabel = BssEnd,
                    EntryLabel = entryChunk.EntryLabel
                },
                VideoModeInfo = useTextMode ? MultibootVideoModeInfo.EgaText80x25 : MultibootVideoModeInfo.Ega640x480x16
            });
            var end = new KernelEndChunk(context.Code.ToString(), End, Bss, BssEnd);
            var stack = new StackChunk(StackSizeInBytes, Stack);
            var heap = new HeapChunk {HeapSizeBytes = 1024*1024*5};
            _sources
                .AddChunk(new Use32Chunk())
                .AddChunk(new BaseAddressChunk(BaseAddress))
                .AddChunk(header)
                .AddChunk(entryChunk)
                .AddChunk(end)
                .AddChunk(stack)
                .AddChunk(heap);
            return _sources.Build();
        }

        private async Task<IFasmResult> Compile(IKernel kernel)
        {
            return await _fasmRunner.CompileAsync(kernel.SourceFile, kernel.KernelBinaryFile);
        }
    }
}
