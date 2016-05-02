using UnityEngine;
using System.Collections;

public class NeuralLayer : MonoBehaviour {

    //This code is based on the Neural network seen in my lecture, given by Samyn Koen

    public int _rows;
    public int _columns;

    float[] _data;

    public NeuralLayer(int rows, int columns)
    {
        _rows = rows;
        _columns = columns;
        _data = new float[rows * columns];
    }

    public float GetCell(int row, int column)
    {
        return _data[row * _columns + column];
    }

    public void SetCell(int row, int column, float value)
    {
        _data[row * _columns + column] = value;
    }

    public bool Mul(ref NeuralLayer toMul, ref NeuralLayer dest)
    {
        if (_columns != toMul._rows)
            return false;
        if (_rows != dest._rows || toMul._columns != dest._columns)
            return false;

        for (int row = 0; row < dest._rows; ++row)
            for (int column = 0; column < dest._columns; ++column)
            {
                //Row of this matrix
                //	DOT
                //column toMul matri
                float r = 0;
                for (int i = 0; i < _columns; ++i)
                {
                    float v1 = GetCell(row, i);
                    float v2 = toMul.GetCell(i, column);
                    r += v1 * v2;
                }
                dest.SetCell(row, column, r);
            }
        return true;
    }
    public void DebugPrint()
    {
        for (int row = 0; row < _rows; ++row)
        {
            for (int column = 0; column < _columns; ++column)
            {
                Debug.Log(GetCell(row, column));
            }
            Debug.Log("\n");
        }
    }

    public bool Sub(ref NeuralLayer toSub, ref NeuralLayer dest)
    {
        if (_columns != toSub._columns || _columns != dest._columns)
            return false;
        if (_rows != toSub._rows || _rows != dest._rows)
            return false;

        for (int r = 0; r < dest._rows; r++)
        {
            for (int c = 0; c < dest._rows; c++)
            {
                float val1 = GetCell(r, c);
                float val2 = toSub.GetCell(r, c);
                dest.SetCell(r, c, val1 - val2);
            }
        }
        return true;
    }



    public void SetRandom(float min, float max)
    {
        for (int r = 0; r < _rows; ++r)
            for (int c = 0; c < _columns; ++c)
                SetCell(r, c, Random.Range(min, max));
    }

    public void Exp()
    {
        for (int r = 0; r < _rows; ++r)
        {
            for (int c = 0; c < _columns; ++c)
            {
                float v = GetCell(r, c);
                float e = Mathf.Exp(v);
                SetCell(r, c, e);
            }
        }
    }

    public float Sum()
    {
        float sum = 0;
        for (int r = 0; r < _rows; ++r)
        {
            for (int c = 0; c < _columns; ++c)
            {
                float v = GetCell(r, c);
                sum += v;
            }
        }
        return sum;
    }

    void Div(float div)
    {
        for (int r = 0; r < _rows; ++r)
        {
            for (int c = 0; c < _columns; ++c)
            {
                float v = GetCell(r, c);
                float nv = v / div;
                SetCell(r, c, nv);
            }
        }
    }

    public void Mul(float mul)
    {
        for (int r = 0; r < _rows; ++r)
        {
            for (int c = 0; c < _columns; ++c)
            {
                float v = GetCell(r, c);
                float nv = v * mul;
                SetCell(r, c, nv);
            }
        }
    }

    public bool Transpose(ref NeuralLayer dest)
    {
        if (_columns != dest._rows || _rows != dest._columns)
            return false;
        for (int r = 0; r < _rows; ++r)
        {
            for (int c = 0; c < _columns; ++c)
            {
                float v = GetCell(r, c);
                dest.SetCell(c, r, v);
            }
        }
        return true;
    }

    public bool Copy(ref NeuralLayer dest)
    {
        if (_rows == dest._rows && _columns == dest._columns)
        {
            for (int r = 0; r < _rows; ++r)
            {
                for (int c = 0; c < _columns; ++c)
                {
                    float v = GetCell(r, c);
                    dest.SetCell(r, c, v);
                }
            }
            return true;
        }
        return false;
    }

    public void Sigmoid()
    {
        for (int r = 0; r < _rows; ++r)
        {
            for (int c = 0; c < _columns; ++c)
            {
                float v = GetCell(r, c);
                v = 1 / (1 + Mathf.Exp(v));
                SetCell(r, c, v);
            }
        }
    }

    public void dSigmoid()
    {
        for (int r = 0; r < _rows; ++r)
        {
            for (int c = 0; c < _columns; ++c)
            {
                float v = GetCell(r, c);
                v = Mathf.Exp(v) / Mathf.Pow(1 + Mathf.Exp(v), 2);
                SetCell(r, c, v);
            }
        }
    }

    public void ElementMul(ref NeuralLayer other)
    {
        if (_rows == other._rows && _columns == other._columns)
        {

            for (int r = 0; r < _rows; ++r)
            {
                for (int c = 0; c < _columns; ++c)
                {
                    float v1 = GetCell(r, c);
                    float v2 = other.GetCell(r, c);
                    SetCell(r, c, v1 * v2);
                }
            }
        }
    }
}

