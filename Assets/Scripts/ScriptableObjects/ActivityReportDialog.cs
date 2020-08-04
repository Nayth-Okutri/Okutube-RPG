using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ActivityReportDialog : DialogConversation
{

    public void InitializeReport()
    {
        if (TheWorld.Instance != null)
        {

            Line ReportLine = new Line();
            for (int i = 0; i < 30; i++)
                ReportLine.text += TheWorld.Instance.GenerateCalendarReport(i);

            lines[0] = ReportLine;

        }

    }
    void OnEnable()
    {
        InitializeReport();
    }


}
