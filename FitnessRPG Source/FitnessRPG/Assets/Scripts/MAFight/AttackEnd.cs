using UnityEngine;

public class AttackEnd : StateMachineBehaviour
{
	private int sender;
    MAFightManager manager;
    int stage;
    GameObject prefab, projectile;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attack", false);
        if (animator.gameObject.GetComponent<PDPlayer>()) return;
        sender = animator.gameObject.tag == "myEnemy" ? 2 : 1;
        manager = sender == 2 ? animator.gameObject.GetComponent<Enemy>().manager : animator.gameObject.GetComponent<Player>().manager;

        if (sender == 2)
        {
            stage = manager.p.EnemyToLoad.stage % 14;
            Vector3 player = manager.player.transform.position;
            int dmg;
            switch (stage)
            {
                case 0:
                    //PredatorAlien stuff
                case 1:
                    //Cat Enemy
                case 2:
                    //Dog Enemy
                    prefab = Resources.Load<GameObject>("Prefabs/MAFights/bomb");
                    projectile = Instantiate(prefab, new Vector3(9.0f, 0.6f, 0), Quaternion.Euler(0, 0, 90));
                    projectile.GetComponent<Projectile>().SetThrower(sender);
                    projectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, 1) * 50);
                    Destroy(projectile, 3);
                    break;
                case 3:
                    //Orc1
                case 4:
                    //Orc2
                case 5:
                    //Orc3
                case 6:
                    //ZombieF
                case 7:
                    //ZombieM
                case 8:
                    //Pumpkin
                case 9:
                    //Dino
                    dmg = manager.enemy.GetComponent<Enemy>().CalcDmg();
                    if (dmg > 0)
                    {
                        
                        manager.SpawnParticle(new Vector3(player.x, player.y + 3,player.z), "Blood");
                        manager.player.GetComponent<Player>().UpdateHealth(dmg);
                    }
                    else
                    {
                        manager.SpawnParticle(new Vector3(player.x, player.y + 3, player.z), "Poof");
                    }
                    break;
                case 10:
                    //AlienGreen
                    break;
                case 11:
                    //Knight
                case 12:
                    //FlyingEye1
                case 13:
                    //FlyingEye2
                    dmg = manager.enemy.GetComponent<Enemy>().CalcDmg();
                    if (dmg > 0)
                    {

                        manager.SpawnParticle(new Vector3(player.x, player.y + 3, player.z), "Blood");
                        manager.player.GetComponent<Player>().UpdateHealth(dmg);
                    }
                    else
                    {
                        manager.SpawnParticle(new Vector3(player.x, player.y + 3, player.z), "Poof");
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            //player stuff
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.gameObject.GetComponent<PDPlayer>()) return;
        if (sender == 2)
        {
            switch (stage)
            {
                case 0:
                    //PredatorAlien stuff
                    prefab = Resources.Load<GameObject>("Prefabs/MAFights/laser");
                    projectile = Instantiate(prefab, new Vector3(7.2f, 0f, 0), Quaternion.identity);
                    projectile.GetComponent<Laser>().setThrower(sender);
                    Destroy(projectile, 3);
                    break;
                case 1:
                    //Cat Enemy
                case 2:
                    //Dog Enemy
                    break;
                case 3:
                    //Orc1
                    animator.gameObject.GetComponent<Orc1>().Turn();
                    animator.gameObject.GetComponent<Orc1>().back = true;
                    animator.SetBool("Run", true);      //<-----doesnt need on state exit, because it has exit time anyways
                    break;
                case 4:
                    //Orc2
                    animator.gameObject.GetComponent<Orc2>().Turn();
                    animator.gameObject.GetComponent<Orc2>().back = true;
                    animator.SetBool("Run", true);      //<-----doesnt need on state exit, because it has exit time anyways
                    break;
                case 5:
                    //Orc3
                    animator.gameObject.GetComponent<Orc3>().Turn();
                    animator.gameObject.GetComponent<Orc3>().back = true;
                    animator.SetBool("Run", true);      //<-----doesnt need on state exit, because it has exit time anyways
                    break;
                case 6:
                    //ZombieF
                    animator.gameObject.GetComponent<ZombieF>().Turn();
                    animator.gameObject.GetComponent<ZombieF>().back = true;
                    animator.SetBool("Run", true);      //<-----doesnt need on state exit, because it has exit time anyways
                    break;
                case 7:
                    //ZombieM
                    animator.gameObject.GetComponent<ZombieM>().Turn();
                    animator.gameObject.GetComponent<ZombieM>().back = true;
                    animator.SetBool("Run", true);      //<-----doesnt need on state exit, because it has exit time anyways
                    break;
                case 8:
                    //Pumpkin
                    break;
                case 9:
                    //Dino
                    break;
                case 10:
                    //AlienGreen
                    prefab = Resources.Load<GameObject>("Prefabs/MAFights/laser");
                    projectile = Instantiate(prefab, new Vector3(7.2f, 0f, 0), Quaternion.identity);
                    projectile.GetComponent<Laser>().setThrower(sender);
                    Destroy(projectile, 3);
                    break;
                case 11:
                    //Knight
                    animator.gameObject.GetComponent<Knight1>().Turn();
                    animator.gameObject.GetComponent<Knight1>().back = true;
                    animator.SetBool("Run", true);      //<-----doesnt need on state exit, because it has exit time anyways
                    break;
                case 12:
                    //FlyingEye1
                    animator.gameObject.GetComponent<FlyingEye1>().Turn();
                    animator.gameObject.GetComponent<FlyingEye1>().back = true;
                    animator.SetBool("Run", true);      //<-----doesnt need on state exit, because it has exit time anyways
                    break;
                case 13:
                    //FlyingEye2
                    animator.gameObject.GetComponent<FlyingEye2>().Turn();
                    animator.gameObject.GetComponent<FlyingEye2>().back = true;
                    animator.SetBool("Run", true);      //<-----doesnt need on state exit, because it has exit time anyways
                    break;
                default:
                    break;
            }
        }
        else
        {
            //player stuff
        }
    }
}
