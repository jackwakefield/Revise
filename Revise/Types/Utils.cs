using System.Numerics;

namespace Revise.Types
{
    public class Utils
    {
        public enum BlendOperation
        {
            D3DTOP_DISABLE = 1,
            D3DTOP_SELECTARG1 = 2,
            D3DTOP_SELECTARG2 = 3,
            D3DTOP_MODULATE = 4,
            D3DTOP_MODULATE2X = 5,
            D3DTOP_MODULATE4X = 6,
            D3DTOP_ADD = 7,
            D3DTOP_ADDSIGNED = 8,
            D3DTOP_ADDSIGNED2X = 9,
            D3DTOP_SUBTRACT = 10,
            D3DTOP_ADDSMOOTH = 11,
            D3DTOP_BLENDDIFFUSEALPHA = 12,
            D3DTOP_BLENDTEXTUREALPHA = 13,
            D3DTOP_BLENDFACTORALPHA = 14,
            D3DTOP_BLENDTEXTUREALPHAPM = 15,
            D3DTOP_BLENDCURRENTALPHA = 16,
            D3DTOP_PREMODULATE = 17,
            D3DTOP_MODULATEALPHA_ADDCOLOR = 18,
            D3DTOP_MODULATECOLOR_ADDALPHA = 19,
            D3DTOP_MODULATEINVALPHA_ADDCOLOR = 20,
            D3DTOP_MODULATEINVCOLOR_ADDALPHA = 21,
            D3DTOP_BUMPENVMAP = 22,
            D3DTOP_BUMPENVMAPLUMINANCE = 23,
            D3DTOP_DOTPRODUCT3 = 24,
            D3DTOP_MULTIPLYADD = 25,
            D3DTOP_LERP = 26,
            D3DTOP_FORCE_DWORD = 0x7fffffff
        }

        public enum Blend
        { 
            D3DBLEND_ZERO = 1,
            D3DBLEND_ONE = 2,
            D3DBLEND_SRCCOLOR = 3,
            D3DBLEND_INVSRCCOLOR = 4,
            D3DBLEND_SRCALPHA = 5,
            D3DBLEND_INVSRCALPHA = 6,
            D3DBLEND_DESTALPHA = 7,
            D3DBLEND_INVDESTALPHA = 8,
            D3DBLEND_DESTCOLOR = 9,
            D3DBLEND_INVDESTCOLOR = 10,
            D3DBLEND_SRCALPHASAT = 11,
            D3DBLEND_BOTHSRCALPHA = 12,
            D3DBLEND_BOTHINVSRCALPHA = 13,
            D3DBLEND_BLENDFACTOR = 14,
            D3DBLEND_INVBLENDFACTOR = 15,
            D3DBLEND_SRCCOLOR2 = 16,
            D3DBLEND_INVSRCCOLOR2 = 17,
            D3DBLEND_FORCE_DWORD = 0x7fffffff
        }

        public static Quaternion QuaternionZero = new Quaternion();

        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            return a + (b - a) * t;
        }

        public static Vector4 Lerp(Vector4 a, Vector4 b, float t)
        {
            return a + (b - a) * t;
        }

        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        /// <summary>
        /// Performs smooth (cubic Hermite) interpolation between 0 and 1.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Smoothstep
        /// </remarks>
        /// <param name="amount">Value between 0 and 1 indicating interpolation amount.</param>
        public static float SmoothStep(float amount)
        {
            return (amount <= 0) ? 0
                : (amount >= 1) ? 1
                : amount * amount * (3 - (2 * amount));
        }
    }
}
