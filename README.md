# Vectorized-MultiLayerPerceptron

Vectorized Multi-Layer Perceptron in VB .NET from C# version https://github.com/HectorPulido/Vectorized-multilayer-neural-network

This is the classical XOR test.

# Versions

02/05/2020 V1.07
- PrintParameters: activation function name displayed
- Faster tests

17/04/2020 V1.06
- Activation function added: Double threshold
- Print output standardized
- Variable names simplification

12/04/2020 V1.05 OOP paradigm
- Weight initialization: rounded, to reproduce functionnal tests exactly
- Generic code: see this repositery: https://github.com/PatriceDargenton/One-Ring-to-rule-them-all

21/03/2020 V1.04 Functionnal tests (2)
- OneIteration: Optional PrintOutput As Boolean = False
- Matrix: Implicit conversion operator !(,) -> Matrix
- clsVectorizedMatrixMLP: ComputeError, ComputeAverageError
- clsVectorizedMatrixMLP: variable names in camelCase

08/03/2020 V1.03 Functionnal tests
- Functionnal tests
- Class clsVectorizedMatrixMLP

25/01/2020 V1.02 Activation functions
- addBiasColumn: bias column can be disabled optionally
- Matrix.ToString(): Print using a ready to code format
- Matrix.Map(): Apply an activation function to each element of the Matrix
- Activation functions

31/12/2019 V1.01 Initial commit: C# -> VB .NET