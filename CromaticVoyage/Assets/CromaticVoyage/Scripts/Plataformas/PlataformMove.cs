using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformMove : MonoBehaviour
{
    public float moveSpeed = 2f;

    // Plataforma 1
    public Transform platform1;
    public Transform pointA1; // Objeto vazio A para a plataforma 1
    public Transform pointB1; // Objeto vazio B para a plataforma 1
    private Vector3 targetPosition1; // Posição de destino atual para a plataforma 1

    // Plataforma 2
    public Transform platform2;
    public Transform pointA2; // Objeto vazio A para a plataforma 2
    public Transform pointB2; // Objeto vazio B para a plataforma 2
    private Vector3 targetPosition2; // Posição de destino atual para a plataforma 2

    private void Start()
    {
        // Inicia movendo as plataformas em direção ao ponto A correspondente
        targetPosition1 = pointA1.position;
        targetPosition2 = pointA2.position;
    }

    void Update()
    {
        // Movimenta a plataforma 1 em direção ao ponto de destino
        platform1.position = Vector3.MoveTowards(platform1.position, targetPosition1, moveSpeed * Time.deltaTime);

        if (platform1.position == pointA1.position)
        {
            targetPosition1 = pointB1.position;
        }
        else if (platform1.position == pointB1.position)
        {
            targetPosition1 = pointA1.position;
        }

        // Movimenta a plataforma 2 em direção ao ponto de destino
        platform2.position = Vector3.MoveTowards(platform2.position, targetPosition2, moveSpeed * Time.deltaTime);

        if (platform2.position == pointA2.position)
        {
            targetPosition2 = pointB2.position;
        }
        else if (platform2.position == pointB2.position)
        {
            targetPosition2 = pointA2.position;
        }
    }
}