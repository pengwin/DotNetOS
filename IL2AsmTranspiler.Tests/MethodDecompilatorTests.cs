using System.Threading.Tasks;
using Autofac;
using Common;
using Common.Tests;
using IL2AsmTranspiler.Interfaces;
using Intrinsic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolsRunner.Interfaces;


namespace IL2AsmTranspiler.Tests
{

    [TestClass]
    public class MethodDecompilatorTests : BaseToolsTest
    {

        #region EmptyMethod Test

        static class EmptyMethodTestKernel
        {
            public static void Entry(){}
        }

        [TestMethod]
        public async Task EmptyMethodTest()
        {
            await AssertKernel(typeof(EmptyMethodTestKernel), builder => { });
        }

        #endregion EmptyMethod Test

        #region ReturnMethod Test

        private static class ReturnMethodTestKernel
        {
            public static void Entry() => Return();
            private static byte Return() => 125;
        }

        [TestMethod]
        public async Task ReturnMethodTest()
        {
            await AssertKernel(typeof(ReturnMethodTestKernel), q =>
                q.WaitForRegisterValue("eax", 125)
                .PrintDebugMesage("Instrisinc method called"));
        }

        #endregion ReturnMethod Test

        #region ReturnMethodWithParams Test

        private static class ReturnMethodWithParamsTestKernel
        {
            public static void Entry() => Return(100, 51);
            private static int Return(int a, int b) => a + b;
        }


        [TestMethod]
        public async Task ReturnMethodWithParamsTest()
        {
            await AssertKernel(typeof(ReturnMethodWithParamsTestKernel), q =>
                q.WaitForRegisterValue("eax", 151)
                .PrintDebugMesage("Instrisinc method called"));
        }

        #endregion ReturnMethodWithParams Test

        #region IntrinsicDecompile Test

        static class IntrinsicDecompileKernel
        {
            public static void Entry() => IntrinsicMethod();

            [Intrinsic("mov [0x7C10], word 0xABCD")]
            private static void IntrinsicMethod(){}
        }


        [TestMethod]
        public async Task IntrinsicDecompileTest()
        {
            await AssertKernel(typeof(IntrinsicDecompileKernel), q =>
                q.WaitForAddressValue(0x7C10, 0xABCD)
                .PrintDebugMesage("Instrisinc method called"));
        }

        #endregion IntrinsicDecompile Test

        #region IntrinsicWithParamsDecompile Test

        static class IntrinsicWithParamsDecompileKernel
        {
            public static void Entry() => IntrinsicMethod(0x7C10, 0xABCD);

            [Intrinsic("pop ebx", "pop eax", "mov [eax], ebx")] 
            private static void IntrinsicMethod(int address, int data) { }
        }

        [TestMethod]
        public async Task IntrinsicWithParamsDecompileTest()
        {
            await AssertKernel(typeof(IntrinsicWithParamsDecompileKernel), q =>
                q.WaitForAddressValue(0x7C10, 0xABCD)
                .PrintDebugMesage("Instrisinc method called"));
        }

        #endregion IntrinsicWithParamsDecompile Test

        #region StaticMethodNoParams Test

        internal static class StaticMethodNoParamsKernel
        {
            public static void Entry() => PrintSymbol();
            private static void PrintSymbol() => IntrinsicMethod();

            [Intrinsic("mov [0xB8000], dword 0x0000ABCD")]
            private static void IntrinsicMethod(){}
        }

        [TestMethod]
        public async Task StaticMethodNoParamsCallTest()
        {
            await AssertKernel(typeof (StaticMethodNoParamsKernel), q =>
                q.WaitForAddressValue(0xB8000, 0xABCD)
                    .PrintDebugMesage("Symbol printed"));
        }

        #endregion StaticMethodNoParams Test

        #region StaticMethodWithParms Test

        internal static class StaticMethodWithParmsKernel
        {
            public static void Entry()
            {
                int address = 0xB8000;
                byte data = 0xC;
                PrintSymbol(address, data);
            }

            private static void PrintSymbol(int address, byte data) => IntrinsicMethod(address, data);

            [Intrinsic("pop ebx", "pop eax","xor ecx, ecx", "mov [eax], ecx", "mov [eax], bl")]
            private static void IntrinsicMethod(int address, byte data){}
        }

        [TestMethod]
        public async Task StaticMethodWithParamsCallTest()
        {
            await AssertKernel(typeof(StaticMethodWithParmsKernel), q =>
              q.WaitForAddressValue(0xB8000, 0xC)
                  .PrintDebugMesage("Symbol printed"));
        }

        #endregion StaticMethodWithParms Test

        #region StaticConstructor Test

        internal static class StaticConstructorKernel
        {
            private static class TestClass
            {
                public static readonly uint SomeField = 0x0F650F48;
            }

            [Intrinsic("pop ebx", "pop eax", "mov [eax], ebx")]
            private static void WriteEax(uint address, uint data) { }

            public static void Entry() => WriteEax(0xB8000, TestClass.SomeField);
        }

        [TestMethod]
        public async Task StaticConstructorTest()
        {
            await AssertKernel(typeof(StaticConstructorKernel), q =>
               q.WaitForAddressValue(0xB8000, 0x0F650F48)
                  .PrintDebugMesage("Constructor called"));
        }

        #endregion StaticConstructor Test

        #region Struct Test

        internal static class StructKernel
        {
            private struct TestStruct
            {
                public uint Field;
            }

            [Intrinsic("pop ebx", "mov eax, [ebx]")]
            private static void WriteEax(TestStruct s) { }

            public static void Entry()
            {
                var s = new TestStruct {Field = 0xABCD};
                //WriteEax(s);
            }
        }

        [TestMethod]
        public async Task StructTest()
        {
            await AssertKernel(typeof(StructKernel), q =>
               q.WaitForRegisterValue("eax", 0xABCD)
                  .PrintDebugMesage("Struct initialized"));
        }

        #endregion StaticConstructor Test
    }
}
