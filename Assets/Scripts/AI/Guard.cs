using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{

    [SerializeField]
    private WaypointManager waypointManager;
    [SerializeField]
    private VariableFloat walkSpeed;
    [SerializeField]
    private VariableBool weaponAvailable;
    [SerializeField]
    private GameObject weapon;

    [SerializeField]
    VariableGameObject target;

    private Selector tree;
    


    private NavMeshAgent agent;
    private Animator animator;

    [SerializeField] private FieldOfView fov;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        fov = GetComponent<FieldOfView>();

        agent.speed = walkSpeed.Value;
    }

    private void Start()
    {
        target = (VariableGameObject)ScriptableObject.CreateInstance("VariableGameObject");
        weaponAvailable = (VariableBool)ScriptableObject.CreateInstance("VariableBool");

        weaponAvailable.Value = false;

        PatrolNode patrolNode = new PatrolNode(waypointManager, agent);
        TargetVisibleNode node_TargetVisible = new TargetVisibleNode(agent.transform, target, fov);
        Invertor node_TargetVisibleInvertor = new Invertor(node_TargetVisible);
        TargetAvailableNode node_TargetAvailable = new TargetAvailableNode(target);
        Invertor node_TargetAvailableInvertor = new Invertor(node_TargetAvailable);

        Sequence sequencePatrol = new Sequence(new List<BTBaseNode> 
        { 
            node_TargetAvailableInvertor, patrolNode, node_TargetVisibleInvertor
        }, "Patrol Sequence");

        //BoolNode weaponAvailableNode = new BoolNode(weaponAvailable);
        //MoveToNode moveToNode = new MoveToNode(weapon.transform, agent, 2f);



        tree = new Selector(new List<BTBaseNode> { sequencePatrol });

        if (Application.isEditor)
        {
            gameObject.AddComponent<ShowNodeTreeStatus>().AddConstructor(transform, tree);
        }
    }

    private void FixedUpdate()
    {
        tree?.Run();
    }
}
