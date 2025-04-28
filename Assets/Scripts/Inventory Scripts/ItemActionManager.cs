using UnityEngine;


/// <summary>
/// �� ���� �Ŵ��� ������Ʈ�� �Ҵ�
/// ������(�Ǵ� ���� ��ü)�� ��ȣ�ۿ��ϰų�, �κ��丮���� �������� ����ϸ� Ư�� �̺�Ʈ�� �߻���Ŵ
/// </summary>
public class ItemActionManager : MonoBehaviour
{
    /// <summary>
    /// �޽����� �ְ�޴°�� ��ų�� ���� �޽��� ���
    /// </summary>
    public static string _SkillMessage = "ActiveSkill";
    private AudioSource mAudioSource;
    [SerializeField] private PlayerMovement mPlayerController;

    [Header("Preloaded objects into the scene")]
    [SerializeField] private GameObject[] mObjects;


    private void Awake()
    {
        mAudioSource = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
    }


    /// <summary>
    /// ������ ��� �̺�Ʈ ȣ��
    /// �� �����۸��� ����Ǵ� ����� ����
    /// </summary>
    /// <param name="item"></param>
    /// <returns>������ ���������� �̷�� ���°�?</returns>
    public bool UseItem(Item item)
    {
        Debug.Log("UseItemEvent");

        switch (item.Type)
        {
            //��ų�� ����Ѱ����?
/*            case ItemType.SKILL:
                {
                    Debug.Log("�̱���"); 
                }*/

            case ItemType.Consumable:
                {
                    switch (item.ItemID)
                    {
                        case (int)ItemCode.PAINKILLER:
                            {
                                SoundManager.Instance.GetSound(mAudioSource, 6, 0.3f);
                                HealthManager.Instance.heal += 20;
                                ///GameManager.Instance.Player.ModifyHP(50);
                                //SoundManager.Instance.PlaySound2D("Food Drink " + SoundManager.Range(1, 4, true));
                                break;
                            }
                    }

                    break;
                }
        }

        return true;
    }

    /// <summary>
    /// �� ������ �������� �ݰų�, NONEŸ��(���� �ʰ�, ��ȣ�ۿ� ����) �����۰� ��ȣ�ۿ��Ѱ�� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="itemID">�ش� �������� �ڵ�</param>
    /// <param name="interactTarget"></param>
    public void InteractionItem(Item item, GameObject interactTarget)
    {
        Debug.Log("InteractionItemEvent");

/*        if (interactTarget.tag == "NPC")
        {
            //NPC FSM ��������
            NPCBase targetNPC = interactTarget.GetComponent<NPCBase>();

            //���� ��ȣ�ۿ��� �Ұ����� ����̶�� ����
            if (!targetNPC.CanInteraction || targetNPC.IsQuotePlaying) { return; }

            //��ȣ�ۿ� �޽��� ����
            MessageDispatcher.Instance.DispatchMessage(0, "", targetNPC.EntityName, "Interaction");
            return;
        }*/
    }

    /// <summary>
    /// �������� ���Կ� ����ϴ°�� �߻��ϴ� �̺�Ʈ�̴�.
    /// </summary>
    /// <param name="slot">��ӵ� ����</param>
    public void SlotOnDropEvent(InventorySlot slot)
    {
        Debug.Log("SlotOnDropEvent");
    }
}

public enum ItemCode
{
    AXE = 1,
    PAINKILLER = 2,
   
  
}
