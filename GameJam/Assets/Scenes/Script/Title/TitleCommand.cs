using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCommand : MonoBehaviour
{
    //�͂��߂���
    public void NewPlayCommand()
    {
        Fade.FadeOut("Stage1");
        Ending_Manager.Reset();//���Z�b�g
    }

    //�R���e�B�j���[
    public void ContinueCommand()
    {
        Fade.FadeOut("Stage1");
    }

    //�����т���
    public void HowToPlayCommand()
    {

    }

    //�I��
    public void ExitCommand()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
    }


    public void ExeCommand(TitleManager.SWITCH command)
    {
        switch (command)
        {
            case TitleManager.SWITCH.NEWPLAY: NewPlayCommand(); break;
            case TitleManager.SWITCH.CONTINUE: ContinueCommand(); break;
            case TitleManager.SWITCH.HOWTOPLAY: HowToPlayCommand(); break;
            case TitleManager.SWITCH.EXIT: ExitCommand(); break;
            default: break;
        }
    }
}
