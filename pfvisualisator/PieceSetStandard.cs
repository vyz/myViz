using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Resources;

namespace pfVisualisator {
    /// <summary>
    /// Piece Set included in the assembly
    /// </summary>
    public class PieceSetStandard : PieceSet {
        /// <summary>Base Path of the resource</summary>
        private string  m_strBasePath;

        /// <summary>
        /// Class Ctor
        /// </summary>
        /// <param name="strName">      Piece set Name</param>
        /// <param name="strBasePath">  Base path in the assembly for this piece set</param>
        /// 
        public PieceSetStandard(string strName, string strBasePath) : base(strName) {
            m_strBasePath   = strBasePath;
        }

        /// <summary>
        /// Gets the pieces name as defined in the assembly
        /// </summary>
        /// <param name="ePiece">   Piece</param>
        /// <returns>
        /// Piece name
        /// </returns>
        protected static string NameFromChessPiece(ChessPiece ePiece) {
            string      strRetVal;

            switch (ePiece) {
            case ChessPiece.Black_Pawn:
                strRetVal   = "Black Pawn";
                break;
            case ChessPiece.Black_Rook:
                strRetVal   = "Black Rook";
                break;
            case ChessPiece.Black_Bishop:
                strRetVal   = "Black Bishop";
                break;
            case ChessPiece.Black_Knight:
                strRetVal   = "Black Knight";
                break;
            case ChessPiece.Black_Queen:
                strRetVal   = "Black Queen";
                break;
            case ChessPiece.Black_King:
                strRetVal   = "Black King";
                break;
            case ChessPiece.White_Pawn:
                strRetVal   = "White Pawn";
                break;
            case ChessPiece.White_Rook:
                strRetVal   = "White Rook";
                break;
            case ChessPiece.White_Bishop:
                strRetVal   = "White Bishop";
                break;
            case ChessPiece.White_Knight:
                strRetVal   = "White Knight";
                break;
            case ChessPiece.White_Queen:
                strRetVal   = "White Queen";
                break;
            case ChessPiece.White_King:
                strRetVal   = "White King";
                break;
            default:
                strRetVal   = null;
                break;
            }
            return(strRetVal);
        }

        /// <summary>
        /// Load the specified piece from XAML
        /// </summary>
        /// <param name="ePiece">       Piece</param>
        protected override UserControl LoadPiece(ChessPiece ePiece) {
            UserControl userControlRetVal;
            Uri         uri;
            string      strUriName;

            strUriName          = m_strBasePath + "/" + NameFromChessPiece(ePiece) + ".xaml";
            uri                 = new Uri(strUriName, UriKind.Relative);
            userControlRetVal   = App.LoadComponent(uri) as UserControl;
            return(userControlRetVal);
        }
    } // Class PieceSetStandard
} // Namespace
