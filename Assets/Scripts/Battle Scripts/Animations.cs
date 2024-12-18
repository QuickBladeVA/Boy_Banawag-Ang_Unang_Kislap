using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    // Public variables to control animations, sprites, and audio
    public Animator pAnimator; // Player's animator
    public Animator eAnimator; // Enemy's animator
    public BattleManager bManager; // Reference to BattleManager

    public SpriteRenderer pSR; // Player's sprite renderer
    public SpriteRenderer eSR; // Enemy's sprite renderer

    public AudioSource pAS; // Player's audio source
    public AudioSource eAS; // Enemy's audio source

    // Audio clips for different actions
    public AudioClip phit;
    public AudioClip ehit;
    public AudioClip dodge;
    public AudioClip super;
    public AudioClip tired;
    public AudioClip defeat;

    // Player and enemy controllers
    PlayerController player;
    PlayerController2 player2;
    AIController enemy;

    // Flags to control sound playbacks
    bool pHitSoundPlayed;
    bool pKnockedSoundPlayed;
    bool pDodgeSoundPlayed;

    bool eHitSoundPlayed;
    bool eKnockedSoundPlayed;
    bool eDodgeSoundPlayed;


    void Start()
    {
        // Get the instance of BattleManager
        bManager = BattleManager.instance;

        // Assign player and enemy references based on game mode
        player = bManager.player;
        if (bManager.isPvP)
        {
            player2 = bManager.player2; // PvP mode has two players
        }
        else
        {
            enemy = bManager.enemy; // PvE mode has one player and one AI enemy
        }

        // Set player and enemy animators from BattleManager
        pAnimator = bManager.playerObj.GetComponent<Animator>();
        eAnimator = bManager.enemyObj.GetComponent<Animator>();

        // Set sprite renderers for both player and enemy
        pSR = pAnimator.gameObject.GetComponent<SpriteRenderer>();
        eSR = eAnimator.gameObject.GetComponent<SpriteRenderer>();

        // Set audio sources for both player and enemy
        pAS = pAnimator.gameObject.GetComponent<AudioSource>();
        eAS = eAnimator.gameObject.GetComponent<AudioSource>();

        // Set default audio volume
        SetAudioVolume(0.5f);

        // Start handling the coroutines for super and tired animations
        HandleCoroutines();


    }


    void Update()
    {
        HandleAnimations(); 
    }

    //Handles coroutines for player and enemy (if PvP mode)
    void HandleCoroutines()
    {
        StartCoroutine(AnimationSuper()); // Player Super animation
        StartCoroutine(AnimationTired()); // Player Tired animation

        if (bManager.isPvP)
        {
            StartCoroutine(AnimationSuper(player2)); // Player 2 Super animation
            StartCoroutine(AnimationTired(player2)); // Player 2 Tired animation

        }
    }


    // Manages all animations for player and enemy
    void HandleAnimations()
    {
        // Handle player animation based on moves and conditions
        HandlePlayerAnimation(bManager.enemyMove, bManager.playerMove, pAnimator, pAS,pSR, phit, player.isHit, player.isKnockedOut,
            player.isTired, player.hasSuper, ref pHitSoundPlayed, ref pKnockedSoundPlayed, ref pDodgeSoundPlayed);

        // If PvP mode, handle player 2's animations
        if (bManager.isPvP)
        {
            HandlePlayerAnimation(bManager.playerMove, bManager.enemyMove, eAnimator, eAS, eSR, ehit, player2.isHit, player2.isKnockedOut,
                player2.isTired, player2.hasSuper, ref eHitSoundPlayed, ref eKnockedSoundPlayed, ref eDodgeSoundPlayed);
        }
        else
        {
            // If PvE mode, handle AI enemy animations
            HandleEnemyAnimation();
        }
    }



    // Handles animations and sounds for player (or player 2 in PvP)
    void HandlePlayerAnimation(Move opponentMove, Move myMove, Animator animator, AudioSource aS,SpriteRenderer sR, AudioClip hit,
        bool isHit, bool isKnocked, bool isTired, bool hasSuper, ref bool isHitPlayed, ref bool isKnockedPlayed, ref bool isDodgePlayed)
    {
        // If player is hit, play hit animation
        if (isHit)
        {
            PlayHitAnimation(opponentMove, animator, ref isHitPlayed, aS, hit);
        }
        // If player is knocked out, play knockout animation
        else if (isKnocked)
        {
            PlayKnockoutAnimation(animator, ref isKnockedPlayed, aS);
        }
        // Otherwise, play movement animations based on player's move
        else
        {
            PlayMovementAnimation(myMove, animator, ref isDodgePlayed, aS);
        }

        // Reset sound flags for the next animation
        ResetPlayerSoundFlags(myMove,sR, isHit, isKnocked, isTired, hasSuper, ref isHitPlayed, ref isKnockedPlayed, ref isDodgePlayed);
    }

    // Handles AI enemy animations and sounds
    void HandleEnemyAnimation()
    {
        // If enemy is hit, play hit animation
        if (enemy.isHit)
        {
            PlayHitAnimation(player.move, eAnimator, ref eHitSoundPlayed, eAS, ehit);
        }
        // If enemy is knocked out, play knockout animation
        else if (enemy.isKnockedOut)
        {
            PlayKnockoutAnimation(eAnimator, ref eKnockedSoundPlayed, eAS);
        }
        // Otherwise, play enemy's movement animations
        else
        {
            PlayMovementAnimation(enemy.move, eAnimator, ref eDodgeSoundPlayed, eAS);
        }

        // Reset sound flags for the next animation
        ResetEnemySoundFlags();
    }



    // Plays the hit animation for player or enemy
    void PlayHitAnimation(Move attackerMove, Animator animator, ref bool hitSoundPlayed, AudioSource audioSource, AudioClip hitClip)
    {
        // Set the hit animation based on the attacker's move
        AnimationHit(attackerMove, animator);

        // Play the hit sound if it hasn't been played yet
        if (!hitSoundPlayed)
        {
            audioSource.PlayOneShot(hitClip);
            hitSoundPlayed = true;
        }
    }

    // Plays the knockout animation for player or enemy
    void PlayKnockoutAnimation(Animator animator, ref bool knockedOutSoundPlayed, AudioSource audioSource)
    {
        // Play the knockout sound if it hasn't been played yet
        if (!knockedOutSoundPlayed)
        {
            AnimationKnocked(animator);
            audioSource.PlayOneShot(defeat);
            knockedOutSoundPlayed = true;
        }
    }

    // Plays the movement animations (e.g., dodge) for player or enemy
    void PlayMovementAnimation(Move move, Animator animator, ref bool dodgeSoundPlayed, AudioSource audioSource)
    {
        // Set movement animation based on the current move
        AnimationState(move, animator);

        // Play the dodge sound if dodging
        if ((move == Move.LDodge || move == Move.RDodge) && !dodgeSoundPlayed)
        {
            audioSource.PlayOneShot(dodge);
            dodgeSoundPlayed = true;
        }
    }



    // Resets sound flags for the player animations
    void ResetPlayerSoundFlags(Move selfMove,SpriteRenderer sR, bool hit, bool knocked, bool tired, bool super, ref bool hitPlayed,
        ref bool knockedPlayed, ref bool dodgePlayed)
    {
        // Reset sound flags for hit, knocked, and dodge
        if (!hit)
            hitPlayed = false;
        if (!knocked)
            knockedPlayed = false;
        if (selfMove != Move.LDodge && selfMove != Move.RDodge)
            dodgePlayed = false;

        // Reset color animation when player is not tired or knocked out
        if (!tired || knocked)
        {
            if (!super)
            {
                AnimationColor(sR, Color.white);
            }
        }
    }

    // Resets sound flags for enemy animations
    void ResetEnemySoundFlags()
    {
        if (!enemy.isHit)
            eHitSoundPlayed = false;
        if (!enemy.isKnockedOut)
            eKnockedSoundPlayed = false;
        if (enemy.move != Move.LDodge && enemy.move != Move.RDodge)
            eDodgeSoundPlayed = false;
    }

    // Sets the audio volume for both player and enemy
    void SetAudioVolume(float volume)
    {
        pAS.volume = volume;
        eAS.volume = volume;
    }



    // Sets the correct animation state based on the move (punch, dodge, idle)
    void AnimationState(Move move, Animator animator)
    {
        switch (move)
        {
            case Move.LPunch:
                animator.SetTrigger("LPunch");
                break;
            case Move.RPunch:
                animator.SetTrigger("RPunch");
                break;
            case Move.LDodge:
                animator.SetTrigger("LDodge");
                break;
            case Move.RDodge:
                animator.SetTrigger("RDodge");
                break;
            default:
                animator.SetTrigger("Idle");
                break;
        }
    }

    // Plays the hit animation for the corresponding side (left or right)
    void AnimationHit(Move attackerMove, Animator animator)
    {
        animator.SetTrigger(attackerMove == Move.LPunch ? "RHit" : "LHit");
    }

    // Plays the knockout animation
    void AnimationKnocked(Animator animator)
    {
        animator.SetTrigger("Knocked");
    }



    // Changes the sprite color
    void AnimationColor(SpriteRenderer sr, Color color)
    {
        sr.color = color;
    }

    // Coroutine to change the color of the sprite for a given duration
    IEnumerator ChangeColorRoutine(SpriteRenderer sr, Color color, float duration)
    {
        AnimationColor(sr, color);
        yield return new WaitForSeconds(duration);
        AnimationColor(sr, Color.white);
    }



    // Coroutine to handle super animation (color and sound) for player 
    IEnumerator AnimationSuper()
    {
        while (true)
        {
            if (player.hasSuper && !player.isTired && !player.isKnockedOut)
            {
                pAS.PlayOneShot(super);
                yield return ChangeColorRoutine(pSR, Color.yellow, 0.3f);
            }
            yield return null;
        }
    }

    // Coroutine to handle tired animation (color and sound) for player
    IEnumerator AnimationTired()
    {
        while (true)
        {
            if (player.isTired && !player.isKnockedOut)
            {
                pAS.PlayOneShot(tired);
                yield return ChangeColorRoutine(pSR, Color.blue, 1f);
            }
            yield return null;
        }
    }
    // Coroutine to handle super animation (color and sound) for player 2
    IEnumerator AnimationSuper(PlayerController2 player)
    {
        while (true)
        {
            if (player.hasSuper && !player.isTired && !player.isKnockedOut)
            {
                eAS.PlayOneShot(super);
                yield return ChangeColorRoutine(eSR, Color.yellow, 0.3f);
            }
            yield return null;
        }
    }

    // Coroutine to handle tired animation (color and sound) for player 2
    IEnumerator AnimationTired(PlayerController2 player)
    {
        while (true)
        {
            if (player.isTired && !player.isKnockedOut)
            {
                eAS.PlayOneShot(tired);
                yield return ChangeColorRoutine(eSR, Color.blue, 1f);
            }
            yield return null;
        }
    }
}
