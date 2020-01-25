
Namespace ActivationFunction

    Public Enum TActivationFunction
        Sigmoid = 1
        HyperbolicTangent = 2

        ''' <summary>
        ''' Exponential Linear Units
        ''' </summary>
        ELU = 3

        ''' <summary>
        ''' Rectified Linear Units (ReLU)
        ''' </summary>
        ReLU = 4

        ''' <summary>
        ''' Rectified Linear Units (ReLU) with sigmoid for derivate
        ''' </summary>
        ReLUSigmoid = 5

        Gaussian = 6
        Sinus = 7
        TangentArc = 8
    End Enum

    ''' <summary>
    ''' Interface for all activation functions
    ''' </summary>
    Public Interface IActivationFunction
        ''' <summary>
        ''' Activation function
        ''' </summary>
        Function Activation#(x#, gain#, center#)
        ''' <summary>
        ''' Derivative function
        ''' </summary>
        Function Derivative#(x#, gain#, center#)
        ''' <summary>
        ''' Is non linear function?
        ''' </summary>
        Function IsNonLinear() As Boolean
    End Interface

    ''' <summary>
    ''' Implements f(x) = Sigmoid
    ''' </summary>
    Public Class SigmoidFunction : Implements IActivationFunction

        Public Function Activation#(x#, gain#, center#) Implements IActivationFunction.Activation
            Return CommonActivation(x, gain, center)
        End Function

        Public Shared Function CommonActivation#(x#, gain#, center#)
            Dim y# = 1 / (1 + Math.Exp(-gain * (x - center)))
            Return y
        End Function

        Public Function Derivative#(x#, gain#, center#) Implements IActivationFunction.Derivative
            Return CommonDerivate#(x#, gain#, center#)
        End Function

        Public Shared Function CommonDerivate#(x#, gain#, center#)

            Dim y#
            If gain = 1 Then
                Dim fx# = CommonActivation(x, gain, center)
                y = fx * (1 - fx)
            Else
                Dim xc# = x - center
                Dim rExp# = Math.Exp(-gain * xc)
                Dim rExpP1# = 1 + rExp
                y = gain * rExp / (rExpP1 * rExpP1)
            End If
            Return y

        End Function

        Public Function IsNonLinear() As Boolean Implements IActivationFunction.IsNonLinear
            Return False
        End Function

    End Class

     ''' <summary>
     ''' Implements f(x) = Hyperbolic Tangent
     ''' </summary>
    Public Class HyperbolicTangentFunction : Implements IActivationFunction

        Public Function Activation#(x#, gain#, center#) Implements IActivationFunction.Activation
            Return CommonActivation(x, gain, center)
        End Function

        Public Shared Function CommonActivation#(x#, gain#, center#)
            Dim xc# = x - center
            Dim y# = 2 / (1 + Math.Exp(-2 * xc)) - 1
            Return y
        End Function

        Public Function Derivative#(x#, gain#, center#) Implements IActivationFunction.Derivative

            Dim y#
            If gain = 1 Then
                Dim fx# = CommonActivation(x, gain, center)
                y = 1 - fx * fx
            Else
                Dim xc# = x - center
                Dim rExp# = Math.Exp(-2 * gain * xc)
                Dim rExpP1# = 1 + rExp
                y = 4 * gain * rExp / (rExpP1 * rExpP1)
            End If
            Return y

        End Function

        Public Function IsNonLinear() As Boolean Implements IActivationFunction.IsNonLinear
            Return False
        End Function

    End Class

    ''' <summary>
    ''' Implements f(x) = Exponential Linear Unit (ELU)
    ''' </summary>
    Public Class ELUFunction : Implements IActivationFunction

        Public Function Activation#(x#, gain#, center#) Implements IActivationFunction.Activation

            Dim xc# = x - center
            Dim y#
            If xc >= 0 Then
                y = xc
            Else
                y = gain * (Math.Exp(xc) - 1)
            End If
            Return y

        End Function

        Public Function Derivative#(x#, gain#, center#) Implements IActivationFunction.Derivative

            If gain < 0 Then Return 0

            Dim y#
            If x >= 0 Then
                y = 1
            Else
                y = x + gain
            End If

            Return y

        End Function

        Public Function IsNonLinear() As Boolean Implements IActivationFunction.IsNonLinear
            Return False
        End Function

    End Class

    ''' <summary>
    ''' Implements Rectified Linear Unit (ReLU)
    ''' </summary>
    Public Class ReluFunction : Implements IActivationFunction
        Public Function Activation#(x#, gain#, center#) Implements IActivationFunction.Activation
            Dim xc# = x - center
            Return Math.Max(xc * gain, 0)
        End Function
        Public Function Derivative#(x#, gain#, center#) Implements IActivationFunction.Derivative
            If x >= center Then Return gain
            Return 0
        End Function
        Public Function IsNonLinear() As Boolean Implements IActivationFunction.IsNonLinear
            Return True
        End Function
    End Class

    ''' <summary>
    ''' Implements Rectified Linear Unit (ReLU) with sigmoid for derivate
    ''' </summary>
    Public Class ReluSigmoidFunction : Implements IActivationFunction
        Public Function Activation#(x#, gain#, center#) Implements IActivationFunction.Activation
            Dim xc# = x - center
            Return Math.Max(xc * gain, 0)
        End Function
        Public Function Derivative#(x#, gain#, center#) Implements IActivationFunction.Derivative
            Return SigmoidFunction.CommonDerivate(x, gain, center) ' Sigmoid derivative
        End Function
        Public Function IsNonLinear() As Boolean Implements IActivationFunction.IsNonLinear
            Return False
        End Function
    End Class

    ''' <summary>
    ''' Implements f(x) = Gaussian
    ''' </summary>
    Public Class GaussianFunction : Implements IActivationFunction

        Public Function Activation#(x#, gain#, center#) Implements IActivationFunction.Activation

            Dim xc# = x - center
            Dim xg# = -gain * xc * xc
            Dim y#
            'Const expMax# = 50
            'If xg > expMax Then
            '    y = 1
            'ElseIf xg < -expMax Then
            '    y = 0
            'Else
                y = Math.Exp(xg)
            'End If
            Return y

        End Function

        Public Function Derivative#(x#, gain#, center#) Implements IActivationFunction.Derivative

            Dim xc# = x - center
            Dim g2# = gain * gain
            Dim exp# = Math.Exp(-g2 * xc * xc)
            Dim y# = -2 * g2 * xc * exp
            Return y

        End Function

        Public Function IsNonLinear() As Boolean Implements IActivationFunction.IsNonLinear
            Return False
        End Function

    End Class

    ''' <summary>
    ''' Implements f(x) = sin(x)
    ''' </summary>
    Public Class SinusFunction : Implements IActivationFunction
        Public Function Activation#(x#, gain#, center#) Implements IActivationFunction.Activation
            Dim y# = gain * Math.Sin(x - center)
            Return y
        End Function
        Public Function Derivative#(x#, gain#, center#) Implements IActivationFunction.Derivative
            Dim y# = gain * Math.Cos(x - center)
            Return y
        End Function
        Public Function IsNonLinear() As Boolean Implements IActivationFunction.IsNonLinear
            Return False
        End Function
    End Class

    ''' <summary>
    ''' Implements f(x) = ArcTangent(x)
    ''' </summary>
    Public Class ArcTangentFunction : Implements IActivationFunction
        Public Function Activation#(x#, gain#, center#) Implements IActivationFunction.Activation
            Dim y# = gain * Math.Atan(gain * (x - center))
            Return y
        End Function
        Public Function Derivative#(x#, gain#, center#) Implements IActivationFunction.Derivative
            Dim xc# = x - center
            ' https://www.wolframalpha.com/input/?i=arctan(alpha+*+x)+derivative
            Dim y# = gain / (1 + gain * gain * xc * xc)
            Return y
        End Function
        Public Function IsNonLinear() As Boolean Implements IActivationFunction.IsNonLinear
            Return False
        End Function
    End Class

End Namespace