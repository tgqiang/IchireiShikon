# Ichirei Shikon: Puzzle

## Lore
You play as a miko who is tasked with purifying the demonic aura in the realm.
To achieve this, you make use of the powers of Naohi in various manners.

The composition of the spirit (Naohi):
- Aramitama (荒魂, Courage)
~ Association with turbulence

- Nigimitama (和魂, Friendship)
~ Association with tranquility

- Sakimitama (幸魂, Love)
~ Association with fortune

- Kushimitama (奇魂, Wisdom)
~ Association with wonder

Harvest the power of the 4 souls to purify the demonic auras with spiritual power.

# Game Objectives & Setup
- Grid-based map that starts off being partially covered by demonic aura, with various souls scattered in the map at the beginning

- Demonic aura present in the game will spread to adjacent map cells after every set interval

   > Any souls/spirits that are located in a tainted cell will be destroyed instantly.
   
   > If the whole map becomes tainted with demonic aura, you lose.

- Goal is to purify the entire map

   > This is done by forming powerful spirits and using them to purify the map via various ways
   
   > No limit on number of moves, the aim is simply to purify the whole map

- Merging mechanisms
  1. Generally, fusing 3-of-the-same-kind souls together gives rise to a specific type of pure spirit
  2. *These powerful spirits need to be tapped on to trigger their effects
  3. Fuse more than 3 of the same souls to create a specific type of pure spirit of higher tier/level
  
     > Currently capping maximum tier to 4

## Types of spirits
1. _**3x Aramitama soul --> Spirit of Courage**_

   > This spirit explodes in an area around itself.
   > This explosion purifies tainted cells caught within the explosion, but destroys other souls/spirits caught within it as well.

   (for the explosion area, the spirit is always located at the center of it, i.e. the explosion spreads from the spirit itself)

   **Level 1**: Square area of 3x3

   **Level 2**: Square area of 5x5

   **Level 3**: Square area of 7x7

   **Level 4**: Square area of 9x9
   
   **Level 5**: Square area of 11x11



2. _**3x Nigimitama soul --> Spirit of Friendship**_

   > (A) This spirit can amplify the powers of nearby souls. The amplification extent depends on this spirit's level. **(OLD IDEA)**

   ~ Spirit of Courage: increase explosion radius
   
   ~ Spirit of Friendship: increase amplification effects
   
   ~ Spirit of Love: increase number of primitive elements spawned
   
   ~ Spirit of Wisdom: increase area of effect

   OR

   > (B) This spirit casts a buff in an AoE that adds 1 level to affected spirits **(NEW IDEA)**

   **Level 1**: only affects level 1 spirits
   
   **Level 2**: affects level 1~2 spirits
   
   **Level 3**: affects level 1~3 spirits
   
   **Level 4**: affects level 1~4 spirits
   
   ~ Level cap for _Spirit of Friendship_ and _Spirit of Harmony_ is **strictly capped at 4**
   
   ~ All other spirits have level cap of 5. **However, this level cap is only attainable via the Spirit of Friendship buff's level-increment feature.**



3. _**3x Sakimitama soul --> Spirit of Love**_

   > This spirit spawns a number of souls around itself (the type of souls spawned are random)
   
   **Level 1**: 2 souls spawned (at top and bottom cell from middle)
   
   **Level 2**: 4 souls spawned (at left, right, top and bottom cell from middle, i.e. a cross shape)
   
   **Level 3**: 6 souls spawned (at left, right, top, bottom, top-left and bottom-right cell from middle)
   
   **Level 4**: 8 souls spawned (at immediate surrounding cells)
   
   **Level 5**: 8 souls spawned + a random level-1 spirit spawned at center



4. _**3x Kushimitama soul --> Spirit of Wisdom**_

   > This spirit purifies its current cell so immensely, that no demonic aura may spread to it.
   > This effect can be temporary (lasts for X turns, where X is the level of the Spirit of Wisdom that was triggered) or permanent

   (for this spirit, all cells it purifies are invulnerable to demonic auras)
   
   **Level 1**: Purifies 1 cell (the current cell that the spirit is located at)
   
   **Level 2**: Purifies 3 cells (left, right, middle)
   
   **Level 3**: Purifies 5 cells (left, right, top, bottom, middle)
   
   **Level 4**: Purifies 7 cells (left, right, top, bottom, top-left, bottom-right, middle)
   
   **Level 5**: Purifies 9 cells (middle + immediate 8 surrounding cells)



5. N x [Aramitama, Nigimitama, Sakimitama, Kushimitama] --> Spirit of Harmony

   > This spirit contains (approximately) ALL the effects of the 4 different types of spirits, effect degree dependent on spirit's level
   
   - Purifies an area in an explosion (this explosion **does not destroy souls or spirits**)
   - Purified area is made invulnerable to demonic aura
   - Spawns souls at purified areas
   - If a spirit (**that is not Spirit of Harmony or Spirit of Friendship**) is caught in the explosion, they have their levels incremented by 1.
