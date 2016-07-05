using UnityEngine;
using System.Collections;

public class ModeTranslator : MonoBehaviour
{
	public string TranslateFromIndex(int _Index)
	{
		switch(_Index)
		{
		case 0:
			{
				return("mode 0: 720x486 @29.97fps [YUV_10BPP_V210] index:13");
			}
		case 1:
			{
				return("mode 1: 720x486 @29.97fps [YUV_UYVY] index:0");
			}
		case 2:
			{
				return("mode 2: 720x576 @25.00fps [YUV_10BPP_V210] index:14");
			}
		case 3:
			{
				return("mode 3: 720x576 @25.00fps [YUV_UYVY] index:1");
			}
		case 4:
			{
				return("mode 4: 1280x720 @50.00fps [RGB_10BPP] index:31");
			}
		case 5:
			{
				return("mode 5: 1280x720 @50.00fps [YUV_10BPP_V210] index:23");
			}
		case 6:
			{
				return("mode 6: 1280x720 @50.00fps [YUV_UYVY_HDYC] index:10");
			}
		case 7:
			{
				return("mode 7: 1280x720 @59.94fps [RGB_10BPP] index:32");
			}
		case 8:
			{
				return("mode 8: 1280x720 @59.94fps [YUV_10BPP_V210] index:24");
			}
		case 9:
			{
				return("mode 9: 1280x720 @59.94fps [YUV_UYVY_HDYC] index:11");
			}
		case 10:
			{
				return("mode 10: 1280x720 @60.00fps [RGB_10BPP] index:33");
			}
		case 11:
			{
				return("mode 11: 1280x720 @60.00fps [YUV_10BPP_V210] index:25");
			}
		case 12:
			{
				return("mode 12: 1280x720 @60.00fps [YUV_UYVY_HDYC] index:12");
			}
		case 13:
			{
				return("mode 13: 1920x1080 @23.98fps [RGB_10BPP] index:26");
			}
		case 14:
			{
				return("mode 14: 1920x1080 @23.98fps [YUV_10BPP_V210] index:15");
			}
		case 15:
			{
				return("mode 15: 1920x1080 @23.98fps [YUV_UYVY_HDYC] index:2");
			}
		case 16:
			{
				return("mode 16: 1920x1080 @24.00fps [RGB_10BPP] index:27");
			}
		case 17:
			{
				return("mode 17: 1920x1080 @24.00fps [YUV_10BPP_V210] index:16");
			}
		case 18:
			{
				return("mode 18: 1920x1080 @24.00fps [YUV_UYVY_HDYC] index:3");
			}
		case 19:
			{
				return("mode 19: 1920x1080 @25.00fps [RGB_10BPP] index:28");
			}
		case 20:
			{
				return("mode 20: 1920x1080 @25.00fps [YUV_10BPP_V210] index:17");
			}
		case 21:
			{
				return("mode 21: 1920x1080 @25.00fps [YUV_UYVY_HDYC] index:4");
			}
		case 22:
			{
				return("mode 22: 1920x1080 @29.97fps [RGB_10BPP] index:29");
			}
		case 23:
			{
				return("mode 23: 1920x1080 @29.97fps [YUV_10BPP_V210] index:18");
			}
		case 24:
			{
				return("mode 24: 1920x1080 @29.97fps [YUV_UYVY_HDYC] index:5");
			}
		case 25:
			{
				return("mode 25: 1920x1080 @30.00fps [RGB_10BPP] index:30");
			}
		case 26:
			{
				return("mode 26: 1920x1080 @30.00fps [YUV_10BPP_V210] index:19");
			}
		case 27:
			{
				return("mode 27: 1920x1080 @30.00fps [YUV_UYVY_HDYC] index:6");
			}
		case 28:
			{
				return("mode 28: 1920x1080 @50.00fps [YUV_10BPP_V210] index:20");
			}
		case 29:
			{
				return("mode 29: 1920x1080 @50.00fps [YUV_UYVY_HDYC] index:7");
			}
		case 30:
			{
				return("mode 30: 1920x1080 @59.94fps [YUV_10BPP_V210] index:21");
			}
		case 31:
			{
				return("mode 31: 1920x1080 @59.94fps [YUV_UYVY_HDYC] index:8");
			}
		case 32:
			{
				return("mode 32: 1920x1080 @60.00fps [YUV_10BPP_V210] index:22");
			}
		case 33:
			{
				return("mode 33: 1920x1080 @60.00fps [YUV_UYVY_HDYC] index:9");
			}
		default:
			{
				return("Mode not recognised");
			}
				
		}

	}

    public string TranslateFromIndexSmall(int _Index)
    {
        switch(_Index)
        {
            case 0:
                {
                    return("720x486 @29.97fps [YUV_10BPP_V210]");
                }
            case 1:
                {
                    return("720x486 @29.97fps [YUV_UYVY]");
                }
            case 2:
                {
                    return("720x576 @25.00fps [YUV_10BPP_V210]");
                }
            case 3:
                {
                    return("720x576 @25.00fps [YUV_UYVY]");
                }
            case 4:
                {
                    return("1280x720 @50.00fps [RGB_10BPP]");
                }
            case 5:
                {
                    return("1280x720 @50.00fps [YUV_10BPP_V210]");
                }
            case 6:
                {
                    return("1280x720 @50.00fps [YUV_UYVY_HDYC]");
                }
            case 7:
                {
                    return("1280x720 @59.94fps [RGB_10BPP]");
                }
            case 8:
                {
                    return("1280x720 @59.94fps [YUV_10BPP_V210]");
                }
            case 9:
                {
                    return("1280x720 @59.94fps [YUV_UYVY_HDYC]");
                }
            case 10:
                {
                    return("1280x720 @60.00fps [RGB_10BPP]");
                }
            case 11:
                {
                    return("1280x720 @60.00fps [YUV_10BPP_V210]");
                }
            case 12:
                {
                    return("1280x720 @60.00fps [YUV_UYVY_HDYC]");
                }
            case 13:
                {
                    return("1920x1080 @23.98fps [RGB_10BPP]");
                }
            case 14:
                {
                    return("1920x1080 @23.98fps [YUV_10BPP_V210]");
                }
            case 15:
                {
                    return("1920x1080 @23.98fps [YUV_UYVY_HDYC]");
                }
            case 16:
                {
                    return("1920x1080 @24.00fps [RGB_10BPP]");
                }
            case 17:
                {
                    return("1920x1080 @24.00fps [YUV_10BPP_V210]");
                }
            case 18:
                {
                    return("1920x1080 @24.00fps [YUV_UYVY_HDYC]");
                }
            case 19:
                {
                    return("1920x1080 @25.00fps [RGB_10BPP]");
                }
            case 20:
                {
                    return("1920x1080 @25.00fps [YUV_10BPP_V210]");
                }
            case 21:
                {
                    return("1920x1080 @25.00fps [YUV_UYVY_HDYC]");
                }
            case 22:
                {
                    return("1920x1080 @29.97fps [RGB_10BPP]");
                }
            case 23:
                {
                    return("1920x1080 @29.97fps [YUV_10BPP_V210]");
                }
            case 24:
                {
                    return("1920x1080 @29.97fps [YUV_UYVY_HDYC]");
                }
            case 25:
                {
                    return("1920x1080 @30.00fps [RGB_10BPP]");
                }
            case 26:
                {
                    return("1920x1080 @30.00fps [YUV_10BPP_V210]");
                }
            case 27:
                {
                    return("1920x1080 @30.00fps [YUV_UYVY_HDYC]");
                }
            case 28:
                {
                    return("1920x1080 @50.00fps [YUV_10BPP_V210]");
                }
            case 29:
                {
                    return("1920x1080 @50.00fps [YUV_UYVY_HDYC]");
                }
            case 30:
                {
                    return("1920x1080 @59.94fps [YUV_10BPP_V210]");
                }
            case 31:
                {
                    return("1920x1080 @59.94fps [YUV_UYVY_HDYC]");
                }
            case 32:
                {
                    return("1920x1080 @60.00fps [YUV_10BPP_V210]");
                }
            case 33:
                {
                    return("1920x1080 @60.00fps [YUV_UYVY_HDYC]");
                }
            default:
                {
                    return("Mode not recognised");
                }

        }
    }
}
