using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
 /// <summary>
 /// also controls left and right click interaction, could maybe be moved at some point
 /// But MoveCamera is more like PlayerController god script 😀
 /// </summary>
public class MoveCamera : Singleton<MoveCamera>, PlayerController.IPlayerActions
{
    PlayerController controls;
    float speed = 0;
    float _minSpeed = 6f;
    float _maxSpeed = 16f;
    float _acceleration = 8f;
    float _deacceleration = 16f;
    float _zoomSpeed = 0.01f;
    bool startAcceleration = false;
    Vector2 direction;
    Vector2 currentDir;
    float zoom;
    public Action<Vector3> CameraMovedDistance;
    public Camera MainCamera, CardCamera;
    public LayerMask CardMask, Default, BookMask;
    private CardBehaviour cardInHand;
    private HexGridBehaviour currentHightlightedHexGrid;
    private bool hexGridHightlightClickedDown;
    Vector2 mousePosition;
    bool draggingCard = false;
    public Action<bool> OnCardDraggin;
    public GameObject GridHighlight;
    
    public void Update()
    {
        if(startAcceleration)
            speed = Mathf.Clamp(speed + _acceleration * Time.deltaTime, _minSpeed, _maxSpeed);
        else
            speed = Mathf.Clamp(speed - _deacceleration * Time.deltaTime, _minSpeed, _maxSpeed);
        Vector3 StartPos = transform.position;
       
        currentDir = Vector2.Lerp(currentDir, direction,0.1f);
        
        transform.position += new Vector3(currentDir.x * speed * Time.deltaTime, 0, currentDir.y * speed * Time.deltaTime);
        transform.position = ClampCameraInsideSpace(transform.position);

        if (transform.position.y < 3f && zoom > 0)
        {
            zoom = 0;
        }
        if (transform.position.y > 25f && zoom < 0)
        {
            zoom = 0;
        }
        if (Mathf.Abs(zoom) > 0.1f)
        {
            //todo: get the camera by class
            transform.position += transform.GetChild(0).forward * _zoomSpeed *zoom * Time.deltaTime;
            zoom *= 0.9f / (1 + Time.deltaTime);
        }
        else
            zoom = 0f;
        CameraMovedDistance.Invoke(transform.position - StartPos);
    }

    public Vector3 ClampCameraInsideSpace(Vector3 pos, float minX = -25, float maxX = 25, float minZ = -40, float maxZ = 20)
    {
        pos[0] = Mathf.Clamp(pos[0], minX, maxX);
        pos[2] = Mathf.Clamp(pos[2], minZ, maxZ);
        return pos;
    }

    private Component CheckMouseHit()
    {
        //we check cardcamera first because it is alway infront of the main camera.
        Ray ray = CardCamera.ScreenPointToRay(mousePosition);
        
        if (Physics.Raycast(ray, out var book, 10000, BookMask))
        {
            if (book.transform.TryGetComponent<BookButton>(out var bookButton))
            {
                return bookButton;
            }
            return null;
        }
        else if (Physics.Raycast(ray, out var hit, 10000, CardMask))
        {
            return hit.transform.GetComponent<CardBehaviour>();
        }
        else 
        {
            ray = MainCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray,out var hexHit, 10000, Default))
            {
                //do something with the hexfields
                return hexHit.transform.GetComponent<HexGridBehaviour>();
            }
        }
        return null;
    }

    public bool MouseIsOver(Transform obj)
    {
        Ray ray = CardCamera.ScreenPointToRay(mousePosition);
        Physics.Raycast(ray, out var hit, 10000);
        return hit.transform == obj;
    }

    public void OnEnable()
    {
        if (controls == null)
        {
            controls = new PlayerController();
            // Tell the "gameplay" action map that we want to get told about
            // when actions get triggered.
            controls.Player.SetCallbacks(this);
        }
        controls.Player.Enable();
        OnCardDraggin += (x) => draggingCard = x;
    }
    void GridHighLightClicked(bool onDown)
    {
        if (hexGridHightlightClickedDown == onDown) return;
        hexGridHightlightClickedDown = onDown;
        GridHighlight.transform.localScale = onDown ? Vector3.one * 0.9f : Vector3.one;
    }
    public void OnDisable()
    {
        controls.Player.Disable();
        OnCardDraggin -= (x) => draggingCard = x;
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started)
            startAcceleration = true;
        else if (context.canceled)
            startAcceleration = false;

        direction = context.ReadValue<Vector2>();

    }
    public void OnZoom(InputAction.CallbackContext context)
    {
        zoom += context.ReadValue<float>();
    }

    public void OnLeftClick(InputAction.CallbackContext context)    
    {
        //doto drag mouse click
         
        if (context.started)
        {
            var startHit = CheckMouseHit();
            if (startHit is CardBehaviour)
            {
                var card = startHit as CardBehaviour;
                card.IsbeingDragged = true;
                cardInHand = card;
                OnCardDraggin.Invoke(true);
                Physics.Raycast(CardCamera.ScreenPointToRay(mousePosition), out var mousepos, 100, Default);
                CardManager.Instance.MousePos.position = mousepos.point;
                BookManager.Instance.CloseBook();
            }
            if (startHit is BookButton)
            {
                var button = startHit as BookButton;
                button.ClickDown();
            }
            if (startHit is HexGridBehaviour)
            {
                //Starthit is on a hexgrid, 
                GridHighLightClicked(true);
            }
        }
        else if (context.canceled && draggingCard)
        {
            if (cardInHand.IsReadyToBeSpend)
            {
                if (cardInHand is BlueprintCardBehaviour)
                {
                    var blueprint = cardInHand as BlueprintCardBehaviour;
                    //so here we check if the field is okay to build on:
                    //it should only be accepted to build on roads and planes.
                    // AND they should be next to a road or building to available. (TODO)
                    //TODO GetbuildingID have to be an enum, I think
                    if (currentHightlightedHexGrid.Type == GridData.GridType.Building && currentHightlightedHexGrid.BuildingId == 3 || currentHightlightedHexGrid.Type == GridData.GridType.Plane)
                    {
                        blueprint.PayThePrice();
                        currentHightlightedHexGrid.BuildingConstruction(blueprint.GetBuildingId, blueprint.GetTurnsToBuild);
                        
                        CardManager.Instance.DiscardCard(cardInHand);
                    }
                    else
                    {
                        cardInHand.IsbeingDragged = false;
                        CardManager.Instance.OnCardBeingSpendable?.Invoke(cardInHand,false);
                        // cant place on these fields
                    }
                }
                if (cardInHand is ActionCardBehaviour)
                {
                    var actionCard = cardInHand as ActionCardBehaviour;
                    if (ActionCardManager.Instance.GetActionCardAbility(currentHightlightedHexGrid, actionCard.GetAbility))
                    {
                        CardManager.Instance.DiscardCard(cardInHand);
                    }
                    else
                    {
                        cardInHand.IsbeingDragged = false;
                        CardManager.Instance.OnCardBeingSpendable?.Invoke(cardInHand, false);
                    }
                }
            }
            cardInHand.IsbeingDragged = false;
            OnCardDraggin.Invoke(false);
            return;
        }


        if (!context.canceled) return;
        var hit = CheckMouseHit();
        if (hit is null)
            return;
        if (hit is HexGridBehaviour)
        {
            //prevent OnClickUp is activating on grids
            if(hexGridHightlightClickedDown)
                GetHexGridInfo(hit as HexGridBehaviour);
        }
        else if (hit is CardBehaviour)
        {
            //should we even do something here?
            // Show the card info ?

        }
        else if (hit is BookButton)
        {
            hit.gameObject.SendMessage("Activate");
        }
        GridHighLightClicked(false);
    }

    private void GetHexGridInfo(HexGridBehaviour hexGrid)
    {
        BookManager.Instance.OpenBook(hexGrid);
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        //performed is when the click is down and up, I think.
        if (!context.performed) return;
        BookManager.Instance.CloseBook();
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
        //todo, move camera around
    }

    public void OnMousePos(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
        if(!Physics.Raycast(CardCamera.ScreenPointToRay(mousePosition), 100, BookMask | CardMask) && 
            Physics.Raycast(MainCamera.ScreenPointToRay(mousePosition), out var gridHit, 10000, Default))
        {
            if (gridHit.transform.TryGetComponent<HexGridBehaviour>(out var hexGridBehaviour) &&
                hexGridBehaviour != currentHightlightedHexGrid)
            {
                GridHighlight.SetActive(true);
                currentHightlightedHexGrid = hexGridBehaviour;
                GridHighlight.transform.SetPositionAndRotation(currentHightlightedHexGrid.transform.position, currentHightlightedHexGrid.transform.rotation);
                GridHighLightClicked(false);
            }
        }
        else
            GridHighlight.SetActive(false);
        if (draggingCard && cardInHand != null)
        {
            Physics.Raycast(CardCamera.ScreenPointToRay(mousePosition),out var hit, 100,Default);
            CardManager.Instance.MousePos.position = hit.point;
        }
    }
}
