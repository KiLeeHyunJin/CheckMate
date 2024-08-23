
using System;
using UnityEngine;

[Serializable]
public class SensorEffect
{
    [SerializeField] DataType.SensorType sensorType;
    [SerializeField] DataType.SensorActiveType sensorActiveType;
    [SerializeField] float skillTime;
    [SerializeField] float checkTime;

    public SensorItem sensor { set; get; }
    PlayerSensor Owner;
    int idx;
    public void Init(SensorItem _sensor, PlayerSensor _Owner, int _idx)
    {
        sensor = _sensor;
        Owner = _Owner;
        idx = _idx;
        sensorType = _sensor.sensorType;
        sensorActiveType = _sensor.activeType;
        skillTime = sensor.skillTime;
    }               
    public void Update(float _time)
    {
        if (Owner.inGameManager.isGameover)
            return;
        if (sensorActiveType == DataType.SensorActiveType.Ever)
            return;
        sensorEffectCheck(_time);
    }
    public float GetPercent()
    {
        if (checkTime >= skillTime)
            return 1f;
        return checkTime / skillTime;
    }

    public void sensorEffectCheck(float _time)
    {
        if (sensorType == DataType.SensorType.Bluffing)
        {
            if (PlayerCharacter.instance.isShield == true)
                return;
        }
        checkTime += _time;
        if (checkTime >= skillTime) //스킬 발동시간을 넘으면
        {
            if (sensorType == DataType.SensorType.Bluffing)
            {
                if (PlayerCharacter.instance.isShield == true)
                    return;
                if (PlayerCharacter.instance.isShield == false)
                    PlayerCharacter.instance.isShield = true;
            }
            Owner.CompleteSensor(idx); //완료상태로 변경
        }
    }
    public void ResetTime() {   checkTime = 0; }
}
