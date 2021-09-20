# OpenCL Matrices Multiplication

## `MatrixMultiply.fs`

`MatrixMultiply.fs` provides two functions:

* `MultiplyMatricesViaCPU`;
* `MultiplyMatricesViaGPU`.

The first one requires two matrices with determined counts of rows and cols, the second one requires `Brahma.OpenCL` `ComputeProvider`, two matrices and one `int` of a size (at the moment GPU multiplication supports square matrices only).

Kernel used for the GPU function is a straightforward multiplication of floats in global memory. API Reference and documentation of `Brahma.FSharp` is quite ambiguous at best and lacks a lot of entries similar to [Kronos OpenCL Specification](https://www.khronos.org/registry/OpenCL/specs/opencl-1.2.pdf) on actually basic stuff like manipulating local memory and using barriers inside kernels. This turned out to be one of the reasons of being unable to implement a more effective algorithm utilizing local memory of CUs.

## `ContextOpenCL.fs`

All necessary OpenCL manipulations are collected in `ContextOpenCL.fs`. Right now, they consist of:

* `CreateProvider`: creates a provider out of `platformName: string` and `deviceType: DeviceType`;
* `CreateCommandQueue`: creates a command queue out of `provider: Provider` and the first device of this provider inside.
