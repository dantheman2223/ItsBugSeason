﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CamaraChange : MonoBehaviour
{
    public GameObject[] ubicaciones;
    GameObject cam;
    public int activeCam = 0;
    int maxCam;
    public float speedMax;
    public float speedBase;
    public float speed;
    public float speedRot;
    public bool cambiando = false;
    public Vector3 destino;
    public Quaternion destinoR;
    float distEntreCams;
    float diferenciaRotaciones;
    public LayerMask mask;
    public GameObject textoHormigas;
    public GameObject textoAbejas;
    public GameObject textoCreador;
    public GameObject textoMariposas;
    public GameObject textoAlmacen;
    public GameObject textoGusanos;
    // Use this for initialization
    void Start()
    {
        BotonCamGeneral = GameObject.Find("MoverCentro").gameObject;
        maxCam = ubicaciones.Length;
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        speed = speedBase;
        if (GameObject.Find("TextoHormigas") != null) textoHormigas = GameObject.Find("TextoHormigas").gameObject;
        if (GameObject.Find("TextoAbejas") != null) textoAbejas = GameObject.Find("TextoAbejas");
        if (GameObject.Find("TextoCreador") != null) textoCreador = GameObject.Find("TextoCreador");
        if (GameObject.Find("TextoMariposas") != null) textoMariposas = GameObject.Find("TextoMariposas");
        if (GameObject.Find("TextoAlmacen") != null) textoAlmacen = GameObject.Find("TextoAlmacen");
        if (GameObject.Find("TextoGusanos") != null) textoGusanos = GameObject.Find("TextoGusanos");
    }
    private static CamaraChange _instance;

    public static CamaraChange Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<CamaraChange>();
            }

            return _instance;
        }
    }
    public void MoverDerecha()
    {
        GameManager.Instance.MenuClose();
        activeCam++;
        if (activeCam > maxCam - 1)
        {
            activeCam = 0;
        }
        ChangeCam();
        CheckAll();
    }
    public void MoverIzquierda()
    {
        GameManager.Instance.MenuClose();

        activeCam--;


        if (activeCam < 0)
        {
            activeCam = maxCam - 1;

        }
        ChangeCam(); CheckAll();

    }
    public void VisionGeneral()
    {
        GameManager.Instance.MenuClose();
        if (activeCam != 3)
        {

            activeCam = 3;
            ChangeCam(); CheckAll();

        }
    }
    public void CheckAll()
    {
        if (activeCam == 0)
        {
            SonidoManager.Instance.Play("HormigasFondo");
            SonidoManager.Instance.Stop("GusanosFondo");
            SonidoManager.Instance.Stop("MariposasFondo");
            SonidoManager.Instance.Stop("AbejaFondo");
            textoHormigas.gameObject.SetActive(true);
            textoAbejas.gameObject.SetActive(false);
            if (GameManager.Instance.desbloqueadasMariposas) textoCreador.gameObject.SetActive(false);
            textoMariposas.gameObject.SetActive(false);
            //textoAlmacen.gameObject.SetActive(false);
            textoGusanos.gameObject.SetActive(false);
        }
        else if (activeCam == 1)
        {
            SonidoManager.Instance.Play("GusanosFondo");
            SonidoManager.Instance.Stop("HormigasFondo");
            SonidoManager.Instance.Stop("MariposasFondo");
            SonidoManager.Instance.Stop("AbejaFondo");
            textoHormigas.gameObject.SetActive(false);
            textoAbejas.gameObject.SetActive(false);
            if (GameManager.Instance.desbloqueadasMariposas) textoCreador.gameObject.SetActive(false);
            textoMariposas.gameObject.SetActive(false);
            textoAlmacen.gameObject.SetActive(true);
            textoGusanos.gameObject.SetActive(true);

        }
        else if (activeCam == 2)
        {
            SonidoManager.Instance.Play("MariposasFondo");
            SonidoManager.Instance.Stop("HormigasFondo");
            SonidoManager.Instance.Stop("GusanosFondo");
            SonidoManager.Instance.Stop("AbejaFondo");
            textoHormigas.gameObject.SetActive(false);
            textoAbejas.gameObject.SetActive(false);
            if (GameManager.Instance.desbloqueadasMariposas) textoCreador.gameObject.SetActive(true);
            textoMariposas.gameObject.SetActive(true);
            //textoAlmacen.gameObject.SetActive(false);
            textoGusanos.gameObject.SetActive(false);


        }
        else if (activeCam == 3)
        {
            SonidoManager.Instance.Stop("HormigasFondo");
            SonidoManager.Instance.Stop("GusanosFondo");
            SonidoManager.Instance.Stop("MariposasFondo");
            SonidoManager.Instance.Stop("AbejaFondo");
            textoHormigas.gameObject.SetActive(true);
            textoAbejas.gameObject.SetActive(true);
            if (GameManager.Instance.desbloqueadasMariposas) textoCreador.gameObject.SetActive(true);
            textoMariposas.gameObject.SetActive(true);
            //textoAlmacen.gameObject.SetActive(true);
            textoGusanos.gameObject.SetActive(true);

        }
        else if (activeCam == 4)
        {
            SonidoManager.Instance.Play("AbejaFondo");
            SonidoManager.Instance.Stop("HormigasFondo");
            SonidoManager.Instance.Stop("GusanosFondo");
            SonidoManager.Instance.Stop("MariposasFondo");
            textoHormigas.gameObject.SetActive(false);
            textoAbejas.gameObject.SetActive(true);
            if(GameManager.Instance.desbloqueadasMariposas)textoCreador.gameObject.SetActive(false);
            textoMariposas.gameObject.SetActive(false);
            //textoAlmacen.gameObject.SetActive(false);
            textoGusanos.gameObject.SetActive(false);
        }
    }
    GameObject lastEffect;
    GameObject lastMenuBordes; public GameObject BotonCamGeneral;
    public void SetScale(Transform obj)
    {
        obj.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }
    public void UnSetScale(Transform obj)
    {
        obj.localScale = Vector3.one;
    }
    public GameObject prefabTexto; bool pasadoFlecha = false; bool pasadoFlecha2 = true;
    // Update is called once per frame
    void Update()
    {
        Ray ray2 = FindObjectOfType<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo2;
        if (Physics.Raycast(ray2, out hitInfo2, Mathf.Infinity, mask))
        {

            if (hitInfo2.transform.tag == "InsectoSuelo")
            {
                if (activeCam != 3)
                {

                    Cursor.Instance.SetCursor(true);

                }
                else
                {

                    Cursor.Instance.SetCursor(true);

                }





            }
            else if (hitInfo2.transform.tag == "Edificio")
            {
                Cursor.Instance.SetCursor(true);

            }
            else
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    Cursor.Instance.SetCursor(false);


                }
            }
            if (EventSystem.current.IsPointerOverGameObject())
            {


                if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.name == "Icono1")
                {
                    Cursor.Instance.SetCursor(true);
                }
                if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.name == "Icono2")
                {
                    Cursor.Instance.SetCursor(true);

                }
            }
            if (hitInfo2.transform.tag == "Edificio")
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {

                    foreach (Transform g in hitInfo2.collider.gameObject.GetComponentsInChildren<Transform>(includeInactive: true))
                    {
                        if (g.tag == "Efecto")
                        {
                            if (lastEffect != null) lastEffect.SetActive(false);
                            g.gameObject.SetActive(true);
                            Cursor.Instance.SetCursor(true);
                            lastEffect = g.gameObject;
                        }
                    }
                }

            }
            else
            {





                if (lastMenuBordes != null) lastMenuBordes.transform.localScale = Vector3.one;

                if (lastEffect != null)
                {
                    lastEffect.SetActive(false);
                }
            }



        }



        if (Input.GetKeyUp(KeyCode.D))
        {
            MoverIzquierda();
            if (activeCam == 3)
            {
                BotonCamGeneral.SetActive(false);
            }
            else
            {
                if (!BotonCamGeneral.activeSelf)
                {
                    BotonCamGeneral.SetActive(true);
                }
            }

        }
        if (Input.GetKeyUp(KeyCode.A))
        {

            MoverDerecha();
            if (activeCam == 3)
            {
                BotonCamGeneral.SetActive(false);
            }
            else
            {
                if (!BotonCamGeneral.activeSelf)
                {
                    BotonCamGeneral.SetActive(true);
                }
            }

        }
        if (Input.GetKeyUp(KeyCode.S))
        {

            BotonCamGeneral.SetActive(false);
            VisionGeneral();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = FindObjectOfType<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, mask))
            {
                if (hitInfo.collider.tag == "InsectoSuelo")
                {
                    SonidoManager.Instance.Play("ClickUI");
                    if ((hitInfo.collider.tag == "InsectoSuelo" && hitInfo.collider.GetComponent<Tuto>()))
                    {

                        Destroy(hitInfo.collider.GetComponentInChildren<Tuto>());
                        GameManager.Instance.flechaTuto1.GetComponent<SpriteRenderer>().enabled = true;

                        pasadoFlecha = true;

                    }
                    GameObject.FindObjectOfType<InsectGenerator>().CogerInsecto(hitInfo.collider.gameObject);
                    GameManager.Instance.MenuClose();
                    if (activeCam != 3)
                    {

                        //if ((hitInfo.collider.tag == "InsectoSuelo" && hitInfo.collider.GetComponent<Tuto>()))
                        //{
                        //    print("A");
                        //    Destroy(hitInfo.collider.GetComponentInChildren<Tuto>());
                        //    foreach (Tuto go in GameObject.FindObjectsOfType<Tuto>())
                        //    {
                        //        if (go.GetComponent<SpriteRenderer>() != null) go.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                        //    }
                        //    print(GameObject.FindObjectOfType<Tuto>().name);
                        //    pasadoFlecha = true;

                        //}
                        //GameObject.FindObjectOfType<InsectGenerator>().CogerInsecto(hitInfo.collider.gameObject);
                        //GameManager.Instance.MenuClose();
                    }
                    else
                    {

                        //GameManager.Instance.SetFeedBack("Can´t pick up bugs from here");
                        //if (hitInfo.collider.GetComponent<Tuto>())
                        //{
                        //    GameObject texto = Instantiate(prefabTexto, hitInfo.point + Vector3.up * 2f, Quaternion.identity);
                        //    texto.GetComponent<TextMesh>().text = "Press A/S/D or screen borders to move";
                        //    texto.GetComponent<TextMesh>().characterSize = 4;
                        //    texto.GetComponent<TextMesh>().fontStyle = FontStyle.Bold;

                        //}
                    }





                }
                else if (hitInfo.collider.tag == "Edificio")
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        SonidoManager.Instance.Play("ClickUI");
                        if (pasadoFlecha2 == true && hitInfo.collider.GetComponent<Manzano>() && GameManager.Instance.flechatuto2.gameObject.GetComponent<SpriteRenderer>().enabled == true)
                        {
                            if (activeCam != 0) GameManager.Instance.MenuOpen(hitInfo.collider.gameObject);
                            GameManager.Instance.flechatuto2.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                            FindObjectOfType<SeasonManager>().started = true;
                        }
                        if (pasadoFlecha == true && hitInfo.collider.GetComponent<Hormiguero>() && GameManager.Instance.flechaTuto1.gameObject.GetComponent<SpriteRenderer>().enabled == true)
                        {
                            if (activeCam != 0) GameManager.Instance.MenuOpen(hitInfo.collider.gameObject);
                            pasadoFlecha2 = true;
                            GameManager.Instance.flechaTuto1.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                            GameManager.Instance.flechatuto2.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                        }
                        GameManager.Instance.MenuOpen(hitInfo.collider.gameObject);
                    }
                }
                else
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {

                        if (GameManager.Instance.menuBlock.activeSelf || GameManager.Instance.menuCompras.activeSelf) SonidoManager.Instance.Play("BotonesUI");
                        GameManager.Instance.MenuClose();
                    }
                }


            }
            if (EventSystem.current.IsPointerOverGameObject())
            {


                if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.name == "Icono1")
                {
                    if (GameManager.Instance.icono1Lleno == "Manzano")
                    {

                        GameManager.Instance.MenuOpen(FindObjectOfType<Manzano>().gameObject); GameManager.Instance.MenuOpen(FindObjectOfType<Manzano>().gameObject);

                    }
                    if (GameManager.Instance.icono1Lleno == "G")
                    {
                        GameManager.Instance.MenuOpen(FindObjectOfType<Gusanero>().gameObject); GameManager.Instance.MenuOpen(FindObjectOfType<Gusanero>().gameObject);
                    }
                    if (GameManager.Instance.icono1Lleno == "P")
                    {
                        GameManager.Instance.MenuOpen(FindObjectOfType<Panal>().gameObject); GameManager.Instance.MenuOpen(FindObjectOfType<Panal>().gameObject);
                    }
                    if (GameManager.Instance.icono1Lleno == "M")
                    {
                        GameManager.Instance.MenuOpen(FindObjectOfType<Mariposero>().gameObject); GameManager.Instance.MenuOpen(FindObjectOfType<Mariposero>().gameObject);
                    }
                    if (GameManager.Instance.icono1Lleno == "F")
                    {
                        GameManager.Instance.MenuOpen(FindObjectOfType<FlorHub>().gameObject); GameManager.Instance.MenuOpen(FindObjectOfType<FlorHub>().gameObject);

                    }
                }
                if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.name == "Icono2")
                {
                    if (GameManager.Instance.icono2Lleno == "Manzano")
                    {
                        GameManager.Instance.MenuOpen(FindObjectOfType<Manzano>().gameObject); GameManager.Instance.MenuOpen(FindObjectOfType<Manzano>().gameObject);

                    }
                    if (GameManager.Instance.icono2Lleno == "G")
                    {
                        GameManager.Instance.MenuOpen(FindObjectOfType<Gusanero>().gameObject); GameManager.Instance.MenuOpen(FindObjectOfType<Gusanero>().gameObject);
                    }
                    if (GameManager.Instance.icono2Lleno == "P")
                    {
                        GameManager.Instance.MenuOpen(FindObjectOfType<Panal>().gameObject); GameManager.Instance.MenuOpen(FindObjectOfType<Panal>().gameObject);
                    }
                    if (GameManager.Instance.icono2Lleno == "M")
                    {
                        GameManager.Instance.MenuOpen(FindObjectOfType<Mariposero>().gameObject); GameManager.Instance.MenuOpen(FindObjectOfType<Mariposero>().gameObject);
                    }
                    if (GameManager.Instance.icono2Lleno == "F")
                    {
                        GameManager.Instance.MenuOpen(FindObjectOfType<FlorHub>().gameObject); GameManager.Instance.MenuOpen(FindObjectOfType<FlorHub>().gameObject);
                    }
                }
            }

        }
        if (cambiando == true)
        {
            GameManager.Instance.ActVolumen();
            speed += 100 * Time.deltaTime;
            float actualD = Vector3.Distance(cam.transform.position, destino);
            float actualT = actualD / speed;
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, destino, speed * Time.deltaTime);
            //cam.transform.rotation =Quaternion.Euler((actualD / distEntreCams) * diferenciaRotaciones * this.transform.rotation.eulerAngles);
            cam.transform.rotation = Quaternion.RotateTowards(cam.transform.rotation, destinoR, speedRot * Time.deltaTime);
            if (Vector3.Distance(cam.transform.position, destino) < 0.3f * distEntreCams)
            {
                if (speed - Time.deltaTime * 250 > 200) speed -= Time.deltaTime * 250;

            }
            if (cam.transform.position == destino && cam.transform.rotation == destinoR)
            {
                cambiando = false;
                if (activeCam != 3)
                {
                    GameManager.Instance.DesactVolumen();
                }
                speed = speedBase;
            }
        }
    }
    public void ChangeCam()
    {
        cambiando = true;
        destino = ubicaciones[activeCam].gameObject.transform.position;
        destinoR = ubicaciones[activeCam].gameObject.transform.rotation;
        distEntreCams = Vector3.Distance(cam.transform.position, destino);
        diferenciaRotaciones = Quaternion.Angle(this.transform.rotation, destinoR);
        //foreach (GameObject go in Camaras)
        //{
        //    go.SetActive(false);
        //    if (go == Camaras[activeCam].gameObject)
        //    {
        //        go.SetActive(true);
        //    }
        //}
    }
}
