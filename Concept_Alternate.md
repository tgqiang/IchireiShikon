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

   > Any souls that are located in a tainted cell will be destroyed instantly.

   > Any spirits that are located in a tainted cell will be corrupted. Corrupted spirits taint cells when triggered.
   
   > If the whole map becomes tainted with demonic aura, you lose.

- Goal is to purify the entire map

   > This is done by forming powerful spirits and using them to purify the map via various ways
   
   > No limit on number of moves, the aim is simply to purify the whole map

- Merging mechanisms
  1. Generally, fusing 3-of-the-same-kind souls together gives rise to a specific type of pure spirit
  2. *These powerful spirits need to be tapped on to trigger their effects
  3. Fuse more than 3 of the same souls to create a specific type of pure spirit of higher tier/level
  
     > Currently capping maximum tier to 4
  4. A spirit of higher level, when triggered once, will have its level decremented once. When the spirit is at level 1, triggering it will cause it to disappear.

## Types of spirits
1. _**3x Aramitama soul --> Spirit of Courage**_

   > This spirit explodes in an area around itself.
   > This explosion purifies tainted cells caught within the explosion, but destroys other souls/spirits caught within it as well.

   (for the explosion area, the spirit is always located at the center of it, i.e. the explosion spreads from the spirit itself)

   **Level 1**: Square area of 3x3

   **Level 2**: Square area of 5x5

   **Level 3**: Square area of 7x7

   **Level 4**: Square area of 9x9



2. _**3x Nigimitama soul --> Spirit of Friendship**_

   > This spirit spawns a number of souls around itself (the type of souls spawned are random). Where the souls are spawned, that spot is purified too.
   
   **Level 1**: 2 souls spawned (at top and bottom cell from middle)
   
   **Level 2**: 4 souls spawned (at left, right, top and bottom cell from middle, i.e. a cross shape)
   
   **Level 3**: 6 souls spawned (at left, right, top, bottom, top-left and bottom-right cell from middle)
   
   **Level 4**: 8 souls spawned (at immediate surrounding cells)



3. _**3x Sakimitama soul --> Spirit of Love**_

   > This spirit purifies cells in an area, and also purifies spirits that are corrupted.

   **Level 1**: Purifies 3 cells (left, right, middle)
   
   **Level 2**: Purifies 5 cells (left, right, top, bottom, middle)
   
   **Level 3**: Purifies 7 cells (left, right, top, bottom, top-left, bottom-right, middle)
   
   **Level 4**: Purifies 9 cells (middle + immediate 8 surrounding cells)



4. _**3x Kushimitama soul --> Spirit of Wisdom**_

   > This spirit purifies its current cell so immensely, that no demonic aura may spread to it.
   > This effect can be temporary (lasts for X turns, where X is the level of the Spirit of Wisdom that was triggered) or permanent

   (for this spirit, all cells it purifies are invulnerable to demonic auras)
   
   **Level 1**: Purifies 3 cells (left, right, middle)
   
   **Level 2**: Purifies 5 cells (left, right, top, bottom, middle)
   
   **Level 3**: Purifies 7 cells (left, right, top, bottom, top-left, bottom-right, middle)
   
   **Level 4**: Purifies 9 cells (middle + immediate 8 surrounding cells)
