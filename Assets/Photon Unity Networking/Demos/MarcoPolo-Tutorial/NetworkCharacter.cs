using UnityEngine;

public class NetworkCharacter : Photon.MonoBehaviour
{
    Animator anim;

    private Vector3 correctPlayerPos = Vector3.zero; // We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; // We lerp towards this

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(anim.GetBool("Jump"));
            stream.SendNext(anim.GetBool("Punch"));
            stream.SendNext(anim.GetBool("Walk"));
            stream.SendNext(anim.GetBool("Sprint"));
            stream.SendNext(anim.GetBool("Idle"));

            myThirdPersonController myC = GetComponent<myThirdPersonController>();
            stream.SendNext((int)myC._characterState);
        }
        else
        {
            // Network player, receive data
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)stream.ReceiveNext();

            myThirdPersonController myC = GetComponent<myThirdPersonController>();
            myC._characterState = (CharacterState)stream.ReceiveNext();
            anim.SetBool("Jump", (bool)stream.ReceiveNext());
            anim.SetBool("Punch", (bool)stream.ReceiveNext());
            anim.SetBool("Walk", (bool)stream.ReceiveNext());
            anim.SetBool("Idle", (bool)stream.ReceiveNext());
            anim.SetBool("Sprint", (bool)stream.ReceiveNext());
        }
    }
}