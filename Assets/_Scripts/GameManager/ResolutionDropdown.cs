using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionDropdown : MonoBehaviour
{
    public Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;

        // 드롭다운 옵션 초기화
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        // 해상도 옵션 생성 및 현재 해상도 인덱스 저장
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        // 드롭다운 옵션 설정
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // 드롭다운 값 변경 이벤트 핸들러
    public void OnResolutionChanged(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
