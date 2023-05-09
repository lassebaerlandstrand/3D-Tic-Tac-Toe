using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeAI : MonoBehaviour
{

    //[HideInInspector] private bool isActivated = false;
    // Included in WinOrLoss instead
    /*private Dictionary<int?, int> lookupValues = new Dictionary<int?, int>()
    {
        { 1, -10 }, // X
        { 2, 10 }, // O
        { 0, 0 } // Tie
    };*/
    [SerializeField] private int maxDepth = 2;


    private void Awake()
    {
        GlobalVar.nextTurn += aiPlay;
        GlobalVar.turnIndex.Add("AI", GlobalVar.turnIndex.Count);
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    private int a = 0;
    private void aiPlay(object sender, GlobalVar.turnClass e)
    {
        // First time, we just place it in a random spot
        /*if(!GlobalVar.gameCompleted && e.turnIndicator == GlobalVar.turnIndex["AI"]) {
            placeInRandomPlace();
        }else */
        if(!GlobalVar.gameCompleted && e.turnIndicator % GlobalVar.turnIndex.Count == GlobalVar.turnIndex["AI"]) { // Minimax
            int[] indexForBestMove = bestMove(GlobalVar.gridBoxes);

            BoxScript box = GlobalVar.gridBoxes[indexForBestMove[0], indexForBestMove[1], indexForBestMove[2]];
            box.BoxStatus = 2; // Circle
            Utilities.CreateOSymbol(box.gameObject, box.gameObject.transform.position, Vector3.zero, box.gameObject.GetComponent<BoxCollider>().size.x * 0.8f, box.Layer);

            WinOrLoss.checkMatch(box.gameObject);
            GlobalVar.TurnIndicator++;
        }
    }

    // TODO: When all moves are equal, add some randomness
    // Account for depth
    // Difficulty by adding randomness to score, 
    private int[] bestMove(BoxScript[,,] board)
    {
        float maxScore = -Mathf.Infinity;
        float minDepth = Mathf.Infinity;
        List<int[]> indexMoves = new List<int[]>(); // So that the moves become more random (not bottom left as first every time)
        int[] indexMove = new int[3];
        // Loop through and simulate whether that spot is best
        for (int i = 0; i < GlobalVar.dimensions; i++) {
            for (int j = 0; j < GlobalVar.dimensions; j++) {
                for (int k = 0; k < GlobalVar.dimensions; k++) {
                    // Spot is not taken
                    if (board[i, j, k].BoxStatus == 0) {
                        board[i, j, k].BoxStatus = 2; // AI placed circle there
                        float[] scoreArray = minimax(board, board[i, j, k], 0, -Mathf.Infinity, Mathf.Infinity, false);
                        board[i, j, k].BoxStatus = 0; // Undo move

                        float score = scoreArray[0];
                        float newDepth = scoreArray[1];
                        if(score > maxScore) {
                            maxScore = score;
                            minDepth = newDepth;
                            indexMove = new int[] { i, j, k };
                            //indexMoves.Add(new int[] { i, j, k });
                        } else if (score == maxScore && newDepth < minDepth) {
                            minDepth = newDepth;
                            indexMove = new int[] { i, j, k };
                        }
                    }
                }
            }
        }     
        return indexMove;
    }


    private float[] minimax(BoxScript[,,] board, BoxScript lastPlaced, int depth, float alpha, float beta, bool isMaximizing)
    {
        a++;
        if (depth >= maxDepth) {
            return new float[] { -1, depth };
        }

        // Check winner on this board, don't need to run minimax again if one player wins in this state, since the game is over
        int? result = WinOrLoss.checkAIMatch(lastPlaced, GlobalVar.openBoxes - depth + 1);
        if (result != null) {
            return new float[] { (float)result , depth};
        }

        // Minimax
        if (isMaximizing) { // AI tries to maximize score
            float maxScore = -Mathf.Infinity;
            float minDepth = Mathf.Infinity;
            for (int i = 0; i < GlobalVar.dimensions; i++) {
                for (int j = 0; j < GlobalVar.dimensions; j++) {
                    for (int k = 0; k < GlobalVar.dimensions; k++) {
                        if (board[i, j, k].BoxStatus == 0) {
                            board[i, j, k].BoxStatus = 2; // AI placed circle here
                            float[] scoreArray = minimax(board, board[i, j, k], depth + 1, alpha, beta, false);
                            board[i, j, k].BoxStatus = 0; // Undo move

                            float score = scoreArray[0];
                            float newDepth = scoreArray[1];
                            if(score > maxScore) {
                                maxScore = score;
                                minDepth = newDepth;
                            }else if (score == maxScore && newDepth < minDepth) {
                                minDepth = newDepth;
                            }

                            alpha = Mathf.Max(alpha, score);
                            if (beta <= alpha) {
                                break;
                            }
                        }
                    }
                }
            }
            return new float[] {maxScore, minDepth};
        } else { // Player tries to minimize score
            float minScore = Mathf.Infinity;
            float minDepth = Mathf.Infinity;
            for (int i = 0; i < GlobalVar.dimensions; i++) {
                for (int j = 0; j < GlobalVar.dimensions; j++) {
                    for (int k = 0; k < GlobalVar.dimensions; k++) {
                        if (board[i, j, k].BoxStatus == 0) {
                            board[i, j, k].BoxStatus = 1; // Player placed x here
                            float[] scoreArray = minimax(board, board[i, j, k], depth + 1, alpha, beta, true);
                            board[i, j, k].BoxStatus = 0; // Undo move

                            float score = scoreArray[0];
                            float newDepth = scoreArray[1];
                            if (score < minScore) {
                                minScore = score;
                                minDepth = newDepth;
                            } else if (score == minScore && newDepth < minDepth) {
                                minDepth = newDepth;
                            }
                            //minScore = Mathf.Min(minScore, score[0]);
                            beta = Mathf.Min(beta, score);
                            if(beta <= alpha) {
                                break;
                            }
                        }
                    }
                }
            }
            return new float[] { minScore, minDepth };
        }
    }

    private void placeInRandomPlace()
    {
        bool placed = false;
        while (!placed) {
            int x = Random.Range(0, GlobalVar.dimensions);
            int y = Random.Range(0, GlobalVar.dimensions);
            int z = Random.Range(0, GlobalVar.dimensions);
            if (GlobalVar.gridBoxes[x, y, z].isEmpty()) {

                GlobalVar.gridBoxes[x, y, z].BoxStatus = 2; // Circle
                Utilities.CreateOSymbol(GlobalVar.gridBoxes[x, y, z].gameObject, GlobalVar.gridBoxes[x, y, z].gameObject.transform.position, Vector3.zero, GlobalVar.gridBoxes[x, y, z].gameObject.GetComponent<BoxCollider>().size.x * 0.8f, GlobalVar.gridBoxes[x, y, z].Layer);

                WinOrLoss.checkMatch(GlobalVar.gridBoxes[x, y, z].gameObject);
                GlobalVar.TurnIndicator++;
                placed = true;
            }
        }
    }

    private void OnDestroy()
    {
        GlobalVar.nextTurn -= aiPlay;
    }

}
