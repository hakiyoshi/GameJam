using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class OriginalDollyCart : CinemachineDollyCart
{
    public void ResetCartPosition()
    {
        if (m_Path != null)
        {
            m_Position = m_Path.StandardizeUnit(0.0f, m_PositionUnits);
            transform.position = m_Path.EvaluatePositionAtUnit(m_Position, m_PositionUnits);
            transform.rotation = m_Path.EvaluateOrientationAtUnit(m_Position, m_PositionUnits);
        }
    }
}
