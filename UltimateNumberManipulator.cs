using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TheUltimateNumber;
using TheUltimateNumber.Patches;
using Unity.Netcode;
using Unity.XR.CoreUtils;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UltimateNumberManipulator : AnimatedItem
{
    public decimal calculatedValue; 
    readonly static byte TEN = 10;
    readonly static byte HUNDRED = 100;
    readonly static short THOUSAND = 1000;
    readonly static short TEN_THOUSAND = 10000;
    readonly static int HUNDRED_THOUSAND = 100000;
    readonly static int MILLION = 1000000;
    readonly static int TEN_MILLION = 10000000;
    readonly static int HUNDRED_MILLION = 100000000;
    readonly static int BILLION = 1000000000;
    readonly static int INT_LIMIT = 2147483647;
    readonly static float COLLIDER_SCALE_FACTOR = 3.44f;
    public AudioClip? zeroAudio;
    public AudioClip? oneAudio;
    public AudioClip? twoAudio;
    public AudioClip? threeAudio;
    public AudioClip? fourAudio;
    public AudioClip? fiveAudio;
    public AudioClip? sixAudio;
    public AudioClip? sevenAudio;
    public AudioClip? eightAudio;
    public AudioClip? nineAudio;
    public AudioClip? minusAudio;
    public AudioClip? oopsAudio;
    public AudioSource? oopsAudioSource;
    [HideInInspector]
    public bool hasGeneratedValue = false;
    public Transform? ones;
    public Transform? tens;
    public Transform? hundreds;
    public Transform? thousands;
    public Transform? tenthousands;
    public Transform? hundredthousands;
    public Transform? millions;
    public Transform? tenmillions;
    public Transform? hundredmillions;
    public Transform? billions;
    public Transform? questionmark;
    [HideInInspector]
    public Transform[]? numberPlacesArray;
    [HideInInspector]
    public string[]? gameObjectNumberSuffix;
    [HideInInspector]
    GameObject? currentGameObject;
    [HideInInspector]
    ScanNodeProperties? currentScanNodeProperties;
    [HideInInspector]
    public BoxCollider? baseBoxCollider;
    [HideInInspector]
    public int numberID;
    [HideInInspector]
    public readonly NetworkVariable<int> _scrapValueSyncer = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector]
    public bool haventMovedNumbersYet = true;
    [HideInInspector]
    public decimal ScrapValueSyncer { get => _scrapValueSyncer.Value; set { _scrapValueSyncer.Value = (int)value; } }
    //this is awful but a byte[] NetworkVariable caused netcode patcher to crash
    [HideInInspector]
    public byte place1 { get => _place1.Value; set { _place1.Value = (byte)value; } }
    [HideInInspector]
    public byte place2 { get => _place2.Value; set { _place2.Value = (byte)value; } }
    [HideInInspector]
    public byte place3 { get => _place3.Value; set { _place3.Value = (byte)value; } }
    [HideInInspector]
    public byte place4 { get => _place4.Value; set { _place4.Value = (byte)value; } }
    [HideInInspector]
    public byte place5 { get => _place5.Value; set { _place5.Value = (byte)value; } }
    [HideInInspector]
    public byte place6 { get => _place6.Value; set { _place6.Value = (byte)value; } }
    [HideInInspector]
    public byte place7 { get => _place7.Value; set { _place7.Value = (byte)value; } }
    [HideInInspector]
    public byte place8 { get => _place8.Value; set { _place8.Value = (byte)value; } }
    [HideInInspector]
    public byte place9 { get => _place9.Value; set { _place9.Value = (byte)value; } }
    [HideInInspector]
    public byte place10 { get => _place10.Value; set { _place10.Value = (byte)value; } }
    [HideInInspector]
    public readonly NetworkVariable<byte> _place1 = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector]
    public readonly NetworkVariable<byte> _place2 = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector]
    public readonly NetworkVariable<byte> _place3 = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector]
    public readonly NetworkVariable<byte> _place4 = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector]
    public readonly NetworkVariable<byte> _place5 = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector]
    public readonly NetworkVariable<byte> _place6 = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector]
    public readonly NetworkVariable<byte> _place7 = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector]
    public readonly NetworkVariable<byte> _place8 = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector]
    public readonly NetworkVariable<byte> _place9 = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector]
    public readonly NetworkVariable<byte> _place10 = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector]
    public byte[] numbersInOrder;
    [HideInInspector]
    public AudioClip[]? normalAudioArray;
    public List<TheUltimateNumber.SpecialAudioClip> specialAudioClips = new List<TheUltimateNumber.SpecialAudioClip>();
    [HideInInspector]
    public bool HasWaited { get => _hasWaited.Value; set { _hasWaited.Value = value; } } 
    [HideInInspector]
    public bool IsExploding { get => _isExploding.Value; set { _isExploding.Value = value; } } 
    [HideInInspector]
    public readonly NetworkVariable<bool> _hasWaited = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector]
    public readonly NetworkVariable<bool> _isExploding = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector]
    public bool HasUpdatedYet { get => _hasUpdatedYet.Value; set { _hasUpdatedYet.Value = value; } }
    [HideInInspector]
    public readonly NetworkVariable<bool> _hasUpdatedYet = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public List<Material> normalMat;
    public List<Material> orangeMat;
    [HideInInspector]
    public bool previously672 = false;
    public Transform sixTransform;
    public Transform sevenTransform;
    public Transform twoTransform;
    [HideInInspector]
    public bool needToFixValue = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        //initialize a bunch of stuff
        currentGameObject = this.gameObject;
        baseBoxCollider = this.gameObject.GetComponent<BoxCollider>();
        numberID = currentGameObject.GetInstanceID();
        Transform currentScanNode = transform.Find("ScanNode");
        GameObject currentScanNodeGO = currentScanNode.gameObject;
        currentScanNodeProperties = currentScanNodeGO.GetComponent<ScanNodeProperties>();
        numbersInOrder = new byte[10] {place1, place2, place3, place4, place5, place6, place7, place8, place9, place10};

        //find transforms
        numberPlacesArray = new Transform[10] { ones, tens, hundreds, thousands, tenthousands, hundredthousands, millions, tenmillions, hundredmillions, billions };
        gameObjectNumberSuffix = new string[10] { "one", "ten", "hundred", "thousand", "10thousand", "100thousand", "million", "10million", "100million", "billion" };
        normalAudioArray = new AudioClip[10] { zeroAudio, oneAudio, twoAudio, threeAudio, fourAudio, fiveAudio, sixAudio, sevenAudio, eightAudio, nineAudio};
        oopsAudioSource.clip = oopsAudio;
        if (IsServer)
        {
            IsExploding = false;
            HasWaited = false;
            HasUpdatedYet = false;
            StartCoroutine(WaitTenSeconds());
        }
        questionmark.gameObject.SetActive(false);
        foreach(Transform i in numberPlacesArray)
        {
            i.gameObject.SetActive(true);
        }
        oopsAudioSource.volume = TheUltimateNumber.TheUltimateNumber.UltimateConfig.numberAudioSourceVolume.Value;
        itemAudio.volume = TheUltimateNumber.TheUltimateNumber.UltimateConfig.numberAudioSourceVolume.Value;
        oopsAudioSource.minDistance = TheUltimateNumber.TheUltimateNumber.UltimateConfig.numberAudioSourceMinDistance.Value;
        itemAudio.maxDistance = TheUltimateNumber.TheUltimateNumber.UltimateConfig.numberAudioSourceMaxDistance.Value;
        oopsAudioSource.maxDistance = TheUltimateNumber.TheUltimateNumber.UltimateConfig.numberAudioSourceMaxDistance.Value;
        itemAudio.minDistance = TheUltimateNumber.TheUltimateNumber.UltimateConfig.numberAudioSourceMinDistance.Value;
        if (IsServer)
        {
            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Server! Subscribing");
            ScrapSyncCheck.ScrapSyncedEvent += FixValue;
        }
        else
        {
            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Not server");
        }
        TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("start ran lol");
    }
    void FixValue(object sender, EventArgs e)
    {
        needToFixValue = true;
        return;
    }
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (needToFixValue == true)
        {
            if (!HasUpdatedYet)
            {
                if (_scrapValueSyncer == null)
                {
                    Console.WriteLine("_scrapValueSyncer doesn't exist!");
                }
                TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Fixing number value!");
                int scrapValueOld = scrapValue;
                TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Old value is " + scrapValueOld + " new will be " + calculatedValue);
                scrapValue = (int)calculatedValue;
                currentScanNodeProperties.subText = ("Value: $" + calculatedValue);
                currentScanNodeProperties.scrapValue = (int)calculatedValue;
                this.ScrapValueSyncer = (int)calculatedValue;
                syncTotalScrapValueServerRpc((int)calculatedValue, scrapValueOld);
                TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Done fixing value");
                HasUpdatedYet = true;
                needToFixValue = false;
            }
            else
            {
                TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Not updating because it's already been updated! " + HasUpdatedYet);
            }
        }
        if (ScrapValueSyncer == 0 && HasWaited == true && IsExploding == false)
        {
            StartCoroutine(PlayExplodeNumberSound());
        }
        if(IsServer)
        {
            if(ScrapValueSyncer != calculatedValue)
            {
                ScrapValueSyncer = calculatedValue; 
            }
            if ((!GameNetworkManager.Instance.gameHasStarted && StartOfRound.Instance.gameStats.daysSpent == 0 && !hasGeneratedValue) || (GameNetworkManager.Instance.gameHasStarted && !hasGeneratedValue))
            {
                TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Calculating scrap value as this scrap just spawned in");
                calculatedValue = CalcValue();
                this.scrapValue = (int)calculatedValue;
                currentScanNodeProperties.subText = ("Value: $" + calculatedValue);
                currentScanNodeProperties.scrapValue = (int)calculatedValue;
                syncTotalScrapValueServerRpc((int)calculatedValue);
                MoveNumbers(calculatedValue);
                return;
            }
            hasGeneratedValue = true;
            //if (this.scrapValue == MAGIC_DONT_RECALCULATE_NUMBER)
            //{
            //    Debug.LogWarning("Congratulations! You've run into an annoying issue I'm too lazy to deal with properly. This scrap's value will be recalculated.");
            //    calculatedValue = CalcValue();
            //    this.scrapValue = (int)calculatedValue;
            //    currentScanNodeProperties.subText = ("Value: $" + calculatedValue);
            //    MoveNumbers(calculatedValue);
            //    return;

            //}
            if (calculatedValue != this.scrapValue || haventMovedNumbersYet == true)
            {
                {
                    TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Value of TheUltimateNumber ID " + numberID + " has changed from " + calculatedValue + " to " + this.scrapValue + ". Updating...");
                    calculatedValue = this.scrapValue;
                    currentScanNodeProperties.subText = ("Value: $" + calculatedValue);
                    currentScanNodeProperties.scrapValue = (int)calculatedValue;
                    TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Value of TheUltimateNumber ID " + numberID + " has changed. Updating...");
                    MoveNumbers(calculatedValue);
                    return;
                }
            }
        }
        else if (!IsServer)
        {
            if (ScrapValueSyncer != this.scrapValue || haventMovedNumbersYet == true)
            {
                TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Value of TheUltimateNumber ID " + numberID + " has desynced. Updating...");
                this.scrapValue = (int)ScrapValueSyncer;
                currentScanNodeProperties.subText = ("Value: $" + ScrapValueSyncer);
                currentScanNodeProperties.scrapValue = (int)ScrapValueSyncer;
                MoveNumbers(ScrapValueSyncer);
                hasGeneratedValue = true;
                return;
            }
        }
        
    }

    void MoveNumbers(decimal value)
    {
        bool isNegative = false;
        if (value < 0)
        {
            isNegative = true;
        }
        decimal absoluteValue = System.Math.Abs(value);
        //moving gameobjects to correct positions based on length
        Vector3 vector = new Vector3();
        foreach(Transform i in numberPlacesArray)
        {
            vector = i.localPosition;
            vector.x = -0.3112598f;
            i.localPosition = vector;
        }
        //i should really remake this without copypasting the same block of code a million times... it's so bad
        TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Value of absoluteValue is " + absoluteValue);
        if(absoluteValue < TEN)
        {
            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("TheUltimateNumber ID " + numberID + "'s value has one digit. Moving GameObjects appropriately...");
            //position the ones place
            vector = ones.localPosition;
            vector.x = -0.3112598f;
            ones.localPosition = vector;
            foreach (Transform i in numberPlacesArray)
            {
                if(System.Array.IndexOf(numberPlacesArray, i) == 0)
                {
                    i.gameObject.SetActive(true);
                }
                else
                {
                    i.gameObject.SetActive(false);
                }
            }
            try
            {
                vector = baseBoxCollider.size;
            }
            catch (System.Exception NullReferenceException)
            {
                TheUltimateNumber.TheUltimateNumber.Logger.LogWarning("Couldn't find BoxCollider. Object is likely invalid and will be destroyed.");
                UnityEngine.Object.Destroy(currentGameObject);
                return;
            }
            vector.x = COLLIDER_SCALE_FACTOR * 1;
            baseBoxCollider.size = vector;
        }
        else if (absoluteValue >= TEN && absoluteValue < HUNDRED)
        {
            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("TheUltimateNumber ID " + numberID + "'s value has two digits. Moving GameObjects appropriately...");
            //position the ones place
            vector = ones.localPosition;
            vector.x = -2.03f;
            ones.localPosition = vector;
            //position the tens place
            vector = tens.localPosition;
            vector.x = 1.41f;
            tens.localPosition = vector;
            foreach (Transform i in numberPlacesArray)
            {
                if (System.Array.IndexOf(numberPlacesArray, i) <= 1)
                {
                    i.gameObject.SetActive(true);
                }
                else
                {
                    i.gameObject.SetActive(false);
                }
            }
            vector = baseBoxCollider.size;
            vector.x = COLLIDER_SCALE_FACTOR * 2;
            baseBoxCollider.size = vector;
        }
        else if (absoluteValue >= HUNDRED && absoluteValue < THOUSAND)
        {
            //TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("TheUltimateNumber ID " + numberID + "'s value has three digits. Moving GameObjects appropriately...");
            //position the ones place
            //TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("ones localPosition is " + ones.localPosition + " before changing the vector.");
            vector = ones.localPosition;
            vector.x = -3.75126f;
            ones.localPosition = vector;
            //TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("ones localPosition is " + ones.localPosition);
            //position the tens place
            //TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("tens localPosition is " + tens.localPosition + " before changing the vector.");
            vector = tens.localPosition;
            vector.x = -0.3112599f;
            tens.localPosition = vector;
            //TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("tens localPosition is " + tens.localPosition);
            //position the hundreds place
            //TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("hundreds localPosition is " + hundreds.localPosition + " before changing the vector.");
            vector = hundreds.localPosition;
            vector.x = 3.12874f;
            hundreds.localPosition = vector;
            //TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("hundreds localPosition is " + hundreds.localPosition);
            foreach (Transform i in numberPlacesArray)
            {
                if (System.Array.IndexOf(numberPlacesArray, i) <= 2)
                {
                    i.gameObject.SetActive(true);
                }
                else
                {
                    i.gameObject.SetActive(false);
                }
            }
            vector = baseBoxCollider.size;
            vector.x = COLLIDER_SCALE_FACTOR * 3;
            baseBoxCollider.size = vector;
        }
        else if (absoluteValue >= THOUSAND && absoluteValue < TEN_THOUSAND)
        {
            //TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("TheUltimateNumber ID " + numberID + "'s value has four digits. Moving GameObjects appropriately...");
            //position the ones place
            //TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("ones localPosition is " + ones.localPosition + " before changing the vector.");
            vector = ones.localPosition;
            vector.x = -5.47f;
            ones.localPosition = vector;
            //TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("ones localPosition is " + ones.localPosition);
            //position the tens place
            //TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("tens localPosition is " + tens.localPosition + " before changing the vector.");
            vector = tens.localPosition;
            vector.x = -2.03f;
            tens.localPosition = vector;
            //TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("tens localPosition is " + tens.localPosition);
            //position the hundreds place
            //TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("hundreds localPosition is " + hundreds.localPosition + " before changing the vector.");
            vector = hundreds.localPosition;
            vector.x = 1.41f;
            hundreds.localPosition = vector;
            //TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("hundreds localPosition is " + hundreds.localPosition);
            //position the thousands place
            //TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("thousands localPosition is " + thousands.localPosition + " before changing the vector.");
            vector = thousands.localPosition;
            vector.x = 4.85f;
            thousands.localPosition = vector;
            //TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("thousands localPosition is " + thousands.localPosition);
            foreach (Transform i in numberPlacesArray)
            {
                if (System.Array.IndexOf(numberPlacesArray, i) <= 3)
                {
                    i.gameObject.SetActive(true);
                }
                else
                {
                    i.gameObject.SetActive(false);
                }
            }
            vector = baseBoxCollider.size;
            vector.x = COLLIDER_SCALE_FACTOR * 4;
            baseBoxCollider.size = vector;
        }
        else if (absoluteValue >= TEN_THOUSAND && absoluteValue < HUNDRED_THOUSAND)
        {
            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("TheUltimateNumber ID " + numberID + "'s value has five digits. Moving GameObjects appropriately...");
            //position the ones place
            vector = ones.localPosition;
            vector.x = -7.191259f;
            ones.localPosition = vector;
            //position the tens place
            vector = tens.localPosition;
            vector.x = -3.751259f;
            tens.localPosition = vector;
            //position the hundreds place
            vector = hundreds.localPosition;
            vector.x = -0.3112594f;
            hundreds.localPosition = vector;
            //position the thousands place
            vector = thousands.localPosition;
            vector.x = 3.12874f;
            thousands.localPosition = vector;
            //position the ten thousands place
            vector = tenthousands.localPosition;
            vector.x = 6.56874f;
            tenthousands.localPosition = vector;
            foreach (Transform i in numberPlacesArray)
            {
                if (System.Array.IndexOf(numberPlacesArray, i) <= 4)
                {
                    i.gameObject.SetActive(true);
                }
                else
                {
                    i.gameObject.SetActive(false);
                }
            }
            vector = baseBoxCollider.size;
            vector.x = COLLIDER_SCALE_FACTOR * 5;
            baseBoxCollider.size = vector;
        }
        else if (absoluteValue >= HUNDRED_THOUSAND && absoluteValue < MILLION)
        {
            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("TheUltimateNumber ID " + numberID + "'s value has six digits. Moving GameObjects appropriately...");
            //position the ones place
            vector = ones.localPosition;
            vector.x = -8.91f;
            ones.localPosition = vector;
            //position the tens place
            vector = tens.localPosition;
            vector.x = -5.47f;
            tens.localPosition = vector;
            //position the hundreds place
            vector = hundreds.localPosition;
            vector.x = -2.03f;
            hundreds.localPosition = vector;
            //position the thousands place
            vector = thousands.localPosition;
            vector.x = 1.409999f;
            thousands.localPosition = vector;
            //position the ten thousands place
            vector = tenthousands.localPosition;
            vector.x = 4.849999f;
            tenthousands.localPosition = vector;
            //position the hundred thousands place
            vector = hundredthousands.localPosition;
            vector.x = 8.289999f;
            hundredthousands.localPosition = vector;
            foreach (Transform i in numberPlacesArray)
            {
                if (System.Array.IndexOf(numberPlacesArray, i) <= 5)
                {
                    i.gameObject.SetActive(true);
                }
                else
                {
                    i.gameObject.SetActive(false);
                }
            }
            vector = baseBoxCollider.size;
            vector.x = COLLIDER_SCALE_FACTOR * 6;
            baseBoxCollider.size = vector;
        }
        else if (absoluteValue >= MILLION && absoluteValue < TEN_MILLION)
        {
            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("TheUltimateNumber ID " + numberID + "'s value has seven digits. Moving GameObjects appropriately...");
            //position the ones place
            vector = ones.localPosition;
            vector.x = -10.63126f;
            ones.localPosition = vector;
            //position the tens place
            vector = tens.localPosition;
            vector.x = -7.191259f;
            tens.localPosition = vector;
            //position the hundreds place
            vector = hundreds.localPosition;
            vector.x = -3.751259f;
            hundreds.localPosition = vector;
            //position the thousands place
            vector = thousands.localPosition;
            vector.x = -0.3112596f;
            thousands.localPosition = vector;
            //position the ten thousands place
            vector = tenthousands.localPosition;
            vector.x = 3.12874f;
            tenthousands.localPosition = vector;
            //position the hundred thousands place
            vector = hundredthousands.localPosition;
            vector.x = 6.56874f;
            hundredthousands.localPosition = vector;
            //position the millions place
            vector = millions.localPosition;
            vector.x = 10.00874f;
            millions.localPosition = vector;
            foreach (Transform i in numberPlacesArray)
            {
                if (System.Array.IndexOf(numberPlacesArray, i) <= 6)
                {
                    i.gameObject.SetActive(true);
                }
                else
                {
                    i.gameObject.SetActive(false);
                }
            }
            vector = baseBoxCollider.size;
            vector.x = COLLIDER_SCALE_FACTOR * 7;
            baseBoxCollider.size = vector;
        }
        else if (absoluteValue >= TEN_MILLION && absoluteValue < HUNDRED_MILLION)
        {
            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("TheUltimateNumber ID " + numberID + "'s value has eight digits. Moving GameObjects appropriately...");
            //position the ones place
            vector = ones.localPosition;
            vector.x = -12.35f;
            ones.localPosition = vector;
            //position the tens place
            vector = tens.localPosition;
            vector.x = -8.91f;
            tens.localPosition = vector;
            //position the hundreds place
            vector = hundreds.localPosition;
            vector.x = -5.47f;
            hundreds.localPosition = vector;
            //position the thousands place
            vector = thousands.localPosition;
            vector.x = -2.03f;
            thousands.localPosition = vector;
            //position the ten thousands place
            vector = tenthousands.localPosition;
            vector.x = 1.41f;
            tenthousands.localPosition = vector;
            //position the hundred thousands place
            vector = hundredthousands.localPosition;
            vector.x = 4.849999f;
            hundredthousands.localPosition = vector;
            //position the millions place
            vector = millions.localPosition;
            vector.x = 8.29f;
            millions.localPosition = vector;
            //position the ten millions place
            vector = tenmillions.localPosition;
            vector.x = 11.73f;
            tenmillions.localPosition = vector;
            foreach (Transform i in numberPlacesArray)
            {
                if (System.Array.IndexOf(numberPlacesArray, i) <= 7)
                {
                    i.gameObject.SetActive(true);
                }
                else
                {
                    i.gameObject.SetActive(false);
                }
            }
            vector = baseBoxCollider.size;
            vector.x = COLLIDER_SCALE_FACTOR * 8;
            baseBoxCollider.size = vector;
        }
        else if (absoluteValue >= HUNDRED_MILLION && absoluteValue < BILLION)
        {
            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("TheUltimateNumber ID " + numberID + "'s value has nine digits. Moving GameObjects appropriately...");
            //position the ones place
            vector = ones.localPosition;
            vector.x = -14.07126f;
            ones.localPosition = vector;
            //position the tens place
            vector = tens.localPosition;
            vector.x = -10.63126f;
            tens.localPosition = vector;
            //position the hundreds place
            vector = hundreds.localPosition;
            vector.x = -7.19126f;
            hundreds.localPosition = vector;
            //position the thousands place
            vector = thousands.localPosition;
            vector.x = -3.75126f;
            thousands.localPosition = vector;
            //position the ten thousands place
            vector = tenthousands.localPosition;
            vector.x = -0.31126f;
            tenthousands.localPosition = vector;
            //position the hundred thousands place
            vector = hundredthousands.localPosition;
            vector.x = 3.128739f;
            hundredthousands.localPosition = vector;
            //position the millions place
            vector = millions.localPosition;
            vector.x = 6.56874f;
            millions.localPosition = vector;
            //position the ten millions place
            vector = tenmillions.localPosition;
            vector.x = 10.00874f;
            tenmillions.localPosition = vector;
            //position the hundred millions place
            vector = hundredmillions.localPosition;
            vector.x = 13.44874f;
            hundredmillions.localPosition = vector;
            foreach (Transform i in numberPlacesArray)
            {
                if (System.Array.IndexOf(numberPlacesArray, i) <= 8)
                {
                    i.gameObject.SetActive(true);
                }
                else
                {
                    i.gameObject.SetActive(false);
                }
            }
            vector = baseBoxCollider.size;
            vector.x = COLLIDER_SCALE_FACTOR * 9;
            baseBoxCollider.size = vector;
        }
        else if (absoluteValue >= BILLION)
        {
            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("TheUltimateNumber ID " + numberID + "'s value has ten digits. Moving GameObjects appropriately...");
            //position the ones place
            vector = ones.localPosition;
            vector.x = -15.79f;
            ones.localPosition = vector;
            //position the tens place
            vector = tens.localPosition;
            vector.x = -12.35f;
            tens.localPosition = vector;
            //position the hundreds place
            vector = hundreds.localPosition;
            vector.x = -8.909999f;
            hundreds.localPosition = vector;
            //position the thousands place
            vector = thousands.localPosition;
            vector.x = -5.469999f;
            thousands.localPosition = vector;
            //position the ten thousands place
            vector = tenthousands.localPosition;
            vector.x = -2.029999f;
            tenthousands.localPosition = vector;
            //position the hundred thousands place
            vector = hundredthousands.localPosition;
            vector.x = 1.41f;
            hundredthousands.localPosition = vector;
            //position the millions place
            vector = millions.localPosition;
            vector.x = 4.85f;
            millions.localPosition = vector;
            //position the ten millions place
            vector = tenmillions.localPosition;
            vector.x = 8.29f;
            tenmillions.localPosition = vector;
            //position the hundred millions place
            vector = hundredmillions.localPosition;
            vector.x = 11.73f;
            hundredmillions.localPosition = vector;
            //position the billions place
            vector = billions.localPosition;
            vector.x = 15.17f;
            billions.localPosition = vector;
            //activate and deactive gameobjects
            foreach (Transform i in numberPlacesArray)
            {
                i.gameObject.SetActive(true);
            }
            vector = baseBoxCollider.size;
            vector.x = COLLIDER_SCALE_FACTOR * 10;
            baseBoxCollider.size = vector;
        }
        else
        {
            TheUltimateNumber.TheUltimateNumber.Logger.LogError("Something went wrong! TheUltimateNumber ID " + numberID + "'s value is invalid with value " + value);
        }
        //TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Ones: " + ones.localPosition + " Tens: " + tens.localPosition + " Hundreds: " + hundreds.localPosition);
        //TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Thousands: " + thousands.localPosition + " TenThousands: " + tenthousands.localPosition + " HundredThousands: " + hundredthousands.localPosition);
        //TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Millions: " + millions.localPosition + " TenMillions: " + tenmillions.localPosition + " HundredMillions: " + hundredmillions.localPosition);
        //TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Billions: " + billions.localPosition);

        //activate the appropriate number gameobjects for each active place
        int divideValue = 1;
        long modValue = 10;
        Transform? highestActive = null;
        byte highestActiveLoop = 0;
        byte numLoops = 0;
        if (IsServer)
        {
            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("We're sever so setting a bunch of placeX values");
            place1 = 255;
            place2 = 255;
            place3 = 255;
            place4 = 255;
            place5 = 255;
            place6 = 255;
            place7 = 255;
            place8 = 255;
            place9 = 255;
            place10 = 255;
        }
        foreach (Transform i in numberPlacesArray)
        {
            if (i.gameObject.activeSelf == true)
            {
                foreach (Transform child in i)
                {
                    child.gameObject.SetActive(false);
                }
                byte numberPlace = (byte)(System.Math.Truncate((absoluteValue % modValue) / divideValue));
                TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Looking for a GameObject named " + numberPlace.ToString() + gameObjectNumberSuffix[numLoops]);
                Transform currentNumber = i.transform.Find(numberPlace.ToString() + gameObjectNumberSuffix[numLoops]);
                TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Found Transform " + currentNumber);
                currentNumber.gameObject.SetActive(true);
                TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Set Transform as active!");
                highestActive = i;
                highestActiveLoop = numLoops;
                if (IsServer)
                {
                    switch (numLoops)
                    {
                        case 0:
                            place1 = numberPlace;
                            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Setting place1 to " + numberPlace);
                            break;
                        case 1:
                            place2 = numberPlace;
                            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Setting place2 to " + numberPlace);
                            break;
                        case 2:
                            place3 = numberPlace;
                            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Setting place3 to " + numberPlace);
                            break;
                        case 3:
                            place4 = numberPlace;
                            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Setting place4 to " + numberPlace);
                            break;
                        case 4:
                            place5 = numberPlace;
                            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Setting place5 to " + numberPlace);
                            break;
                        case 5:
                            place6 = numberPlace;
                            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Setting place6 to " + numberPlace);
                            break;
                        case 6:
                            place7 = numberPlace;
                            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Setting place7 to " + numberPlace);
                            break;
                        case 7:
                            place8 = numberPlace;
                            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Setting place8 to " + numberPlace);
                            break;
                        case 8:
                            place9 = numberPlace;
                            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Setting place9 to " + numberPlace);
                            break;
                        case 9:
                            place10 = numberPlace;
                            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Setting place10 to " + numberPlace);
                            break;
                        default:
                            TheUltimateNumber.TheUltimateNumber.Logger.LogError("Genuinely how did this happen. The numberPlace switch statement broke lol");
                            break;
                    }
                }
                
            }
            divideValue = divideValue * TEN;
            modValue = modValue * TEN;
            numLoops++;
        }
        if (isNegative == true)
        {
            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Looking for a GameObject named -" + gameObjectNumberSuffix[highestActiveLoop]);
            Transform negativeSign = highestActive.transform.Find("-" + gameObjectNumberSuffix[highestActiveLoop]);
            negativeSign.gameObject.SetActive(true);
        }
        if(value == 672)
        {
            sixTransform.gameObject.GetComponent<MeshRenderer>().SetMaterials(orangeMat);
            sevenTransform.gameObject.GetComponent<MeshRenderer>().SetMaterials(orangeMat);
            twoTransform.gameObject.GetComponent<MeshRenderer>().SetMaterials(orangeMat);
            previously672 = true;
        }
        else if(value != 672 && previously672 == true)
        {
            sixTransform.gameObject.GetComponent<MeshRenderer>().SetMaterials(normalMat);
            sevenTransform.gameObject.GetComponent<MeshRenderer>().SetMaterials(normalMat);
            twoTransform.gameObject.GetComponent<MeshRenderer>().SetMaterials(normalMat);
            previously672 = false;
        }
            haventMovedNumbersYet = false;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _scrapValueSyncer.Initialize(this);
        _place1.Initialize(this);
        _place2.Initialize(this);
        _place3.Initialize(this);
        _place4.Initialize(this);
        _place5.Initialize(this);
        _place6.Initialize(this);
        _place7.Initialize(this);
        _place8.Initialize(this);
        _place9.Initialize(this);
        _place10.Initialize(this);
        _hasWaited.Initialize(this);
        _isExploding.Initialize(this);
        _hasUpdatedYet.Initialize(this);
    }
    public int CalcValue()
    {
        //calculate a value for the scrap
        int minrand = 0;
        int maxrand = TEN;
        System.Random rng = new();
        //System.Random rng = new(StartOfRound.Instance.randomMapSeed + 672);

        //i don't know how to do this properly so here's another giant mess of if statements
        //TODO turn this into an array or something to make it less awful
        double randomNumber = (rng.NextDouble() * 100);
        //50% chance to have a maximum value of 100
        if (randomNumber <= TheUltimateNumber.TheUltimateNumber.UltimateConfig.hundredsChance.Value)
        {
            minrand = maxrand;
            maxrand = HUNDRED;
            randomNumber = (rng.NextDouble() * 100);
            //10% chance to have a maximum value of 1k
            if (randomNumber <= TheUltimateNumber.TheUltimateNumber.UltimateConfig.thousandsChance.Value)
            {
                minrand = maxrand;
                maxrand = THOUSAND;
                //10% chance to have a maximum value of 10k
                randomNumber = (rng.NextDouble() * 100);
                if (randomNumber <= TheUltimateNumber.TheUltimateNumber.UltimateConfig.tenThousandsChance.Value)
                {
                    minrand = maxrand;
                    maxrand = TEN_THOUSAND;
                    //10% chance to have a maximum value of 100k
                    randomNumber = (rng.NextDouble() * 100);
                    if (randomNumber <= TheUltimateNumber.TheUltimateNumber.UltimateConfig.hundredThousandsChance.Value)
                    {
                        minrand = maxrand;
                        maxrand = HUNDRED_THOUSAND;
                        //10% chance to have a maximum value of mil
                        randomNumber = (rng.NextDouble() * 100);
                        if (randomNumber <= TheUltimateNumber.TheUltimateNumber.UltimateConfig.millionsChance.Value)
                        {
                            minrand = maxrand;
                            maxrand = MILLION;
                            //10% chance to have a maximum value of 10mil
                            randomNumber = (rng.NextDouble() * 100);
                            if (randomNumber <= TheUltimateNumber.TheUltimateNumber.UltimateConfig.tenMillionsChance.Value)
                            {
                                minrand = maxrand;
                                maxrand = TEN_MILLION;
                                //10% chance to have a maximum value of 100mil
                                randomNumber = (rng.NextDouble() * 100);
                                if (randomNumber <= TheUltimateNumber.TheUltimateNumber.UltimateConfig.hundredMillionsChance.Value)
                                {
                                    minrand = maxrand;
                                    maxrand = HUNDRED_MILLION;
                                    //10% chance to have a maximum value of bil
                                    randomNumber = (rng.NextDouble() * 100);
                                    if (randomNumber <= TheUltimateNumber.TheUltimateNumber.UltimateConfig.billionsChance.Value)
                                    {
                                        minrand = maxrand;
                                        maxrand = BILLION;
                                        //50% chance to have a maximum value of signed integer limit
                                        randomNumber = (rng.NextDouble() * 100);
                                        if (randomNumber <= TheUltimateNumber.TheUltimateNumber.UltimateConfig.intLimitChance.Value)
                                        {
                                            minrand = maxrand;
                                            maxrand = INT_LIMIT;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        int scrapValue = rng.Next(minrand, maxrand);
        while (scrapValue == 0)
        {
            scrapValue = rng.Next(minrand, maxrand);
        }
        //lazy workaround for integer limt being slightly annoying to work with (will probably never be used because of how unlikely the number is to get this high)
        if (scrapValue == (INT_LIMIT - 1))
        {
            TheUltimateNumber.TheUltimateNumber.Logger.LogWarning("Jesus christ. Never expected this code to be used. Good luck with the dice roll.");
            scrapValue = (scrapValue + rng.Next(0, 2));
        }
        TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Calculated a scrap value of " + scrapValue + " for TheUltimateNumber ID " + numberID);
        hasGeneratedValue = true;
        return scrapValue;
    }
    public override void EquipItem()
    {
        base.EquipItem();
        TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Equipping item and trying to play audio");
        if(hasGeneratedValue == true)
        {
            foreach (SpecialAudioClip clip in specialAudioClips)
            {
                if (clip.clip != null && clip.value == ScrapValueSyncer)
                {
                    if(clip.explodeAfter == true)
                    {
                        oopsAudioSource.clip = clip.clip;
                        StartCoroutine(PlayExplodeNumberSound());
                        return;
                    }
                    itemAudio.clip = clip.clip;
                    itemAudio.Play();   
                    return;
                }
            }
            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("No special audio clip matches value");
            StartCoroutine(PlayNormalNumberAudio());
            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Tried to play audio");
        }
    }
    //public async void PlayNormalNumberAudio()
        public IEnumerator PlayNormalNumberAudio()
    {
        TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Trying to play audio");
        sbyte forIndex = 0;
        bool playedMinus = false;
        byte clip1 = place1;
        byte clip2 = place2;
        byte clip3 = place3;
        byte clip4 = place4;
        byte clip5 = place5;
        byte clip6 = place6;
        byte clip7 = place7;
        byte clip8 = place8;
        byte clip9 = place9;
        byte clip10 = place10;
        for (sbyte i = 0; i < 10; i++)
        {
            if (i < 10)
            {
                if (wasInPocket || playerHeldBy == null)
                {
                    TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Item has been dropped/pocketed!");
                    i = (sbyte)(i + TEN);
                    yield return new WaitForSeconds(0f);
                }
                if (playedMinus == false && ScrapValueSyncer < 0)
                {
                    TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Negative! Playing minus audio");
                    itemAudio.clip = minusAudio;
                    itemAudio.Play();
                    i = (sbyte)(i - 1);
                    playedMinus = true;
                    //await Task.Delay((int)itemAudio.clip.length * 1000);
                    yield return new WaitForSeconds(itemAudio.clip.length);
                }
                forIndex = i;
                    switch (i)
                    {
                        case 0:
                            if(clip10 < 10)
                            {
                                itemAudio.clip = normalAudioArray[clip10];
                            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Playing AudioClip " + normalAudioArray[clip10]);
                                itemAudio.Play();
                            //await Task.Delay((int)itemAudio.clip.length * 1000);
                            yield return new WaitForSeconds(itemAudio.clip.length);
                            break;
                            }
                            break;
                        case 1:
                            if (clip9 < 10)
                            {
                                itemAudio.clip = normalAudioArray[clip9];
                            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Playing AudioClip " + normalAudioArray[clip9]);
                                itemAudio.Play();
                            //await Task.Delay((int)itemAudio.clip.length * 1000);
                            yield return new WaitForSeconds(itemAudio.clip.length);
                            break;
                            }
                            break;
                        case 2:
                            if (clip8 < 10)
                            {
                                itemAudio.clip = normalAudioArray[clip8];
                            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Playing AudioClip " + normalAudioArray[clip8]);
                                itemAudio.Play();
                            //await Task.Delay((int)itemAudio.clip.length * 1000);
                            yield return new WaitForSeconds(itemAudio.clip.length);
                            break;
                            }
                            break;
                        case 3:
                            if (clip7 < 10)
                            {
                                itemAudio.clip = normalAudioArray[clip7];
                            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Playing AudioClip " + normalAudioArray[clip7]);
                                itemAudio.Play();
                            //await Task.Delay((int)itemAudio.clip.length * 1000);
                            yield return new WaitForSeconds(itemAudio.clip.length);
                            break;
                            }
                            break;
                        case 4:
                            if (clip6 < 10)
                            {
                                itemAudio.clip = normalAudioArray[clip6];
                            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Playing AudioClip " + normalAudioArray[clip6]);
                                itemAudio.Play();
                            //await Task.Delay((int)itemAudio.clip.length * 1000);
                            yield return new WaitForSeconds(itemAudio.clip.length);
                            break;
                            }
                            break;
                        case 5:
                            if (clip5 < 10)
                            {
                                itemAudio.clip = normalAudioArray[clip5];
                            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Playing AudioClip " + normalAudioArray[clip5]);
                                itemAudio.Play();
                            //await Task.Delay((int)itemAudio.clip.length * 1000);
                            yield return new WaitForSeconds(itemAudio.clip.length);
                            break;
                            }
                            break;
                        case 6:
                            if (clip4 < 10)
                            {
                                itemAudio.clip = normalAudioArray[clip4];
                            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Playing AudioClip " + normalAudioArray[clip4]);
                                itemAudio.Play();
                            //await Task.Delay((int)itemAudio.clip.length * 1000);
                            yield return new WaitForSeconds(itemAudio.clip.length);
                            break;
                            }
                            break;
                        case 7:
                            if (clip3 < 10)
                            {
                                itemAudio.clip = normalAudioArray[clip3];
                            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Playing AudioClip " + normalAudioArray[clip3]);
                                itemAudio.Play();
                            //await Task.Delay((int)itemAudio.clip.length * 1000);
                            yield return new WaitForSeconds(itemAudio.clip.length);
                            break;
                            }
                            break;
                        case 8:
                            if (clip2 < 10)
                            {
                                itemAudio.clip = normalAudioArray[clip2];
                            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Playing AudioClip " + normalAudioArray[clip2]);
                                itemAudio.Play();
                            //await Task.Delay((int)itemAudio.clip.length * 1000);
                            yield return new WaitForSeconds(itemAudio.clip.length);
                            break;
                            }
                            break;
                        case 9:
                            if (clip1 < 10)
                            {
                                itemAudio.clip = normalAudioArray[clip1];
                            TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Playing AudioClip " + normalAudioArray[clip1]);
                                itemAudio.Play();
                            //await Task.Delay((int)itemAudio.clip.length * 1000);
                            yield return new WaitForSeconds(itemAudio.clip.length);
                            break;
                            }
                            break;
                        default:
                        //TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("i was equal to " + i + ". Does this make sense?");
                        break;
                    }
            }
            yield return new WaitForSeconds(0f);
        }
    }
    public override void DiscardItem()
    {
        // thanks to Evk on StackOverflow for this code lol idk what im doing
        var ptr = typeof(GrabbableObject).GetMethod("DiscardItem").MethodHandle.GetFunctionPointer();
        var baseDiscard = (Action)Activator.CreateInstance(typeof(Action), this, ptr);
        baseDiscard();
    }

    public override void PocketItem()
    {
        base.PocketItem();
        itemAudio.volume = TheUltimateNumber.TheUltimateNumber.UltimateConfig.numberAudioSourceVolume.Value;
    }
    public IEnumerator WaitTenSeconds()
    {
        for (byte i = 0; i < 2; i++)
        {
            if (i == 1)
            {
                HasWaited = true;
                yield return new WaitForSeconds(0f);
            }
            yield return new WaitForSeconds(10f);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void ExplodeNumberServerRpc()
    {
        ExplodeNumberClientRpc();
    }
    [ClientRpc]
    public void ExplodeNumberClientRpc()
    {
        if (base.isHeld == true)
        {
            base.playerHeldBy.DropAllHeldItems();
        }
        Landmine.SpawnExplosion(base.transform.position, spawnExplosionEffect: true, 5f, 6f, 50, TheUltimateNumber.TheUltimateNumber.UltimateConfig.explosionPhysicsForce.Value);
        UnityEngine.Object.Destroy(base.gameObject);
    }
    public IEnumerator PlayExplodeNumberSound()
    {
        playOopsAudioServerRpc();
        yield return new WaitForSeconds(oopsAudioSource.clip.length);
        ExplodeNumberServerRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    public void playOopsAudioServerRpc()
    {
        playOopsAudioClientRpc();
    }
    [ClientRpc]
    public void playOopsAudioClientRpc()
    {
        IsExploding = true;
        oopsAudioSource.Play();
    }
    [ServerRpc(RequireOwnership = false)]
    public void syncTotalScrapValueServerRpc(float plusval = 0, float minusval = 0)
    {
        syncTotalScrapValueClientRpc(plusval, minusval);
    }
    [ClientRpc]
    public void syncTotalScrapValueClientRpc(float plusval, float minusval)
    {
        TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Total scrap value before adding is " + RoundManager.Instance.totalScrapValueInLevel);
        RoundManager.Instance.totalScrapValueInLevel = RoundManager.Instance.totalScrapValueInLevel + plusval - minusval;
        TheUltimateNumber.TheUltimateNumber.Logger.LogDebug("Total scrap value after adding is " + RoundManager.Instance.totalScrapValueInLevel);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        ScrapSyncCheck.ScrapSyncedEvent -= FixValue;
    }
}
// buh