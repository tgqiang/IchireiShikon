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
   
   **Comments:** AoE of this spirit *may or may not* need to be adjusted, as its mechanics balance itself out.



2. _**3x Nigimitama soul --> Spirit of Friendship**_

   > **Design implementation as of 25/5/2018:** This spirit casts a buff in an AoE **that purifies cells (added on 25/5/2018)** and also adds 1 level to affected spirits **(NEW IDEA)**

   **Level 1**: only affects level 1 spirits
   
   **Level 2**: affects level 1~2 spirits
   
   **Level 3**: affects level 1~3 spirits
   
   **Level 4**: affects level 1~4 spirits
   
   ~ **Design implementation as of 25/5/2018:** AoE size is currently fixed at **3x3 square area**, as an arbitrary start-off point.
   
   ~ **Comments:** Rationale for design implementations/additions: I am presuming that having spirits that do not purify cells at all may lead to players not being likely to use them, since cell-tainting spreads each time a turn is taken and the move-cost for setup for this spirit does not seem to be very worth it.
   
   ~ **Level cap for Spirit of Friendship and Spirit of Harmony is strictly capped at 4, to prevent infinite level-buffing exploitation.**
   
   ~ All other spirits have level cap of 5. **However, this level cap is only attainable via the Spirit of Friendship buff's level-increment feature.**



3. _**3x Sakimitama soul --> Spirit of Love**_

   > This spirit spawns a number of souls around itself (the type of souls spawned are random)
   
   **Level 1**: 2 souls spawned (at top and bottom cell from middle)
   
   **Level 2**: 4 souls spawned (at left, right, top and bottom cell from middle, i.e. a cross shape)
   
   **Level 3**: 6 souls spawned (**Design implementation as of 29/5/2018: 2x top cells, 2x bottom cells, left and right cell from middle, i.e. a long t-shape**)
   
   **Level 4**: 8 souls spawned (**Design implementation as of 29/5/2018: 2x top cells, 2x bottom cells, 2x left cells and 2x right cells from middle, i.e. a wider cross shape**)
   
   **Level 5**: 8 souls spawned + a random level-1 spirit spawned at center
   
   ~ **Comments:**
   
   25/5/2018: Might change spawn positions/formulae to fit current game structure more easily. To be confirmed later on.
   
   29/5/2018: Idea of implementation changes was to ensure that there is either horizontal/vertical symmetry at any state, due to the rectangular nature of the map.



4. _**3x Kushimitama soul --> Spirit of Wisdom**_

   > This spirit purifies its current cell so immensely, that no demonic aura may spread to it.
   > This effect can be temporary (lasts for X turns, where X is the level of the Spirit of Wisdom that was triggered) or permanent
   > **Design implementation as of 25/5/2018:** Cells are purified in a cross-shape of radius R, which means R cells spanning from center cell, both horizontally and vertically, will be purified.

   (for this spirit, all cells it purifies are invulnerable to demonic auras)
   
   **Level 1**: Purifies cells in cross-shape, of radius 1 (the current cell that the spirit is located at)
   
   **Level 2**: Purifies cells in cross-shape, of radius 2 (left, right, top, bottom, middle)
   
   **Level 3**: Purifies cells in cross-shape, of radius 3
   
   **Level 4**: Purifies cells in cross-shape, of radius 4
   
   **Level 5**: Purifies cells in cross-shape, of radius 5
   
   ~ **Rationale for design implementations/additions:** current implementation seems less of a hassle for now based on current game structure, and currently using this as a start-off point. Will modify this where required at a later time, after first playtesting is achieved. Additionally, was thinking of introducing a strategy where players can use this spirit to create "partitions" in the map where tainted tiles cannot spread beyond them.



5. N x [Aramitama, Nigimitama, Sakimitama, Kushimitama] --> Spirit of Harmony

   > This spirit contains (approximately, actually pretty much) ALL the effects of the 4 different types of spirits, effect degree dependent on spirit's level
   
   - Purifies an area in an explosion (this explosion **does not destroy souls or spirits**), AoE follows that of Spirit of Courage.
   - Purified area is made invulnerable to demonic aura
   - Spawns souls at same areas as a Spirit of Love of the corresponding level
   - If a spirit (**that is not a maxed-level Spirit of Harmony or Spirit of Friendship**) is caught in the explosion, they have their levels incremented by 1.
