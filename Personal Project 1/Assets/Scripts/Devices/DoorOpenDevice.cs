using UnityEngine;

public class DoorOpenDevice : BaseDevice
{
    [SerializeField] private Vector3 dPos;

    private bool _open;
    private float _speed = 1f;

    public override void Operate()
    {
        if (_open)
        {
            Vector3 pos = transform.position - dPos;
            // transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * _speed);
            transform.position = pos;
        }
        else
        {
            Vector3 pos = transform.position + dPos;
            transform.position = pos;
            // transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * _speed);
        }
        _open = !_open;
    }

    public void Activate()
    {
        if (!_open)
        {
            Vector3 pos = transform.position + dPos;
            transform.position = pos;
            _open = true;
        }
    }

    public void Deactivate()
    {
        if (_open)
        {
            Vector3 pos = transform.position - dPos;
            transform.position = pos;
            _open = false;
        }
    }
}