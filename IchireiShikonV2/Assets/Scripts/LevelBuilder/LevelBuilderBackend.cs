using UnityEngine;

public class LevelBuilderBackend : MonoBehaviour {

    int[][] levelTilemap = new int[LevelSyntax.LEVEL_TILEMAP_MAX_HEIGHT][];

	void Start () {
        for (int row = 0; row < levelTilemap.Length; row++) {
            levelTilemap[row] = new int[LevelSyntax.LEVEL_TILEMAP_MAX_LENGTH];
        }
	}

    public string GetTilemapAsString() {
        string data = "";

        for (int row = 0; row < LevelSyntax.LEVEL_TILEMAP_MAX_HEIGHT; row++) {
            for (int col = 0; col < LevelSyntax.LEVEL_TILEMAP_MAX_LENGTH; col++) {
                data += levelTilemap[row][col];

                if (col < LevelSyntax.LEVEL_TILEMAP_MAX_LENGTH - 1) data += ",";
            }

            if (row < LevelSyntax.LEVEL_TILEMAP_MAX_HEIGHT - 1) data += "\n";
        }

        Debug.Log(data);
        return data;
    }

    public void UpdateValueAtTileUnit(int value, Vector2Int tileCoords) {
        if (!value.IsBetweenExclusive(LevelSyntax.EMPTY_UNIT, LevelSyntax.MAX_NUMERICAL_REPRESENTATION)) {
            Debug.Log("Invalid value encountered when updating value at tile unit, where value at [" + tileCoords + "] is [" + value + "]");
        } else {
            levelTilemap[tileCoords.x][tileCoords.y] = value;
        }
    }
}
