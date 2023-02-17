﻿using GXPEngine;

public class Player : Unit
{
    public const float DASH_POWER = 120;
    public const float DASH_QUICKNESS = 0.4f;

    private Vector2 _direction;
    private Vector2 _lastDirection;
    private Vector2 _dashDestination;

    private bool _isActive = true;
    private bool _isDashing = false;
    public bool IsImmortal { get => _isDashing; }

    public Player(Vector2 position, Stat hp, Stat ms) : base(position, hp, ms, "Empty", 1, 1) 
    {
        Level.OnPlayerAdded += SubscribeToInput;
        SetOrigin(width / 2, height / 2);
    }
    private void SubscribeToInput()
    {
        int id = Level.Players.Count - 1;
        InputManager.OnUpButtonPressed[id] += () => SetDirection(new Vector2(_direction.x, 1));
        InputManager.OnDownButtonPressed[id] += () => SetDirection(new Vector2(_direction.x, -1));
        InputManager.OnRightButtonPressed[id] += () => SetDirection(new Vector2(1, _direction.y));
        InputManager.OnLeftButtonPressed[id] += () => SetDirection(new Vector2(-1, _direction.y));
        InputManager.OnSpaceButtonPressed[id] += () => StartDash();

        ResetParameters("Player" + id);

        Level.OnPlayerAdded -= SubscribeToInput;
    }
    private void Update()
    {
        if (!ValidateUpdate())
            return;

        if (_isActive)
        {
            if (_isDashing)
                Dash();
            else
                Move(_direction);
        }
    }
    private void SetDirection(Vector2 direction)
    {
        _direction = direction;
        if (direction == Vector2.zero)
            return;

        _lastDirection = direction;
    }
    private void Move(Vector2 direction)
    {
        SetXY
        (
            x + MoveSpeed.CurrentAmount * direction.x * Time.deltaTime,
            y + MoveSpeed.CurrentAmount * direction.y * Time.deltaTime
        );
        _direction = Vector2.zero;
        ClampToBoundaries();
    }
    private void ClampToBoundaries()
    {
        Vector2 center = new Vector2(Game.main.width / 2, Game.main.height / 2);
        Vector2 playerPos = new Vector2(x, y);
        Vector2 clampedPos = Vector2.Distance(playerPos, center) <= Level.MAX_RADIUS ? playerPos : center + (playerPos - center).normalized * Level.MAX_RADIUS;
        SetXY(clampedPos.x, clampedPos.y);
    }
    private void StartDash() 
    {
        _isDashing = true;
        _dashDestination = new Vector2
        (
             x + DASH_POWER * _lastDirection.x,
             y + DASH_POWER * _lastDirection.y
        );
    }
    private void Dash()
    {
        Vector2 interpolatedPosition = GetInterpolatedDashStep();
        SetXY(interpolatedPosition.x, interpolatedPosition.y);
    }
    private Vector2 GetInterpolatedDashStep()
    {
        Vector2 interpolatedDashStep = Vector2.Lerp
        (
            new Vector2(x,y),
            _dashDestination,
            DASH_QUICKNESS
        );
        if (Vector2.Distance(interpolatedDashStep, _dashDestination) < 1)
        {
            _isDashing = false;
            _dashDestination = Vector2.zero;
        }
        return interpolatedDashStep;
    }
}
