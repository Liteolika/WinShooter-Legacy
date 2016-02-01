// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrintLabelDoesNotExistException.cs" company="John Allberg">
//   Copyright �2001-2016 John Allberg
//   
//   This program is free software; you can redistribute it and/or
//   modify it under the terms of the GNU General Public License
//   as published by the Free Software Foundation; either version 2
//   of the License, or (at your option) any later version.
//   
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE. See the
//   GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License
//   along with this program; if not, write to the Free Software
//   Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
// </copyright>
// <summary>
//   Summary description for PrintLabelDoesNotExistException.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.WinShooterServerRemoting
{
    /// <summary>
    /// Summary description for PrintLabelDoesNotExistException.
    /// </summary>
    public class PrintLabelDoesNotExistException : System.ApplicationException
    {
        /// <summary>
        /// Base
        /// </summary>
        public PrintLabelDoesNotExistException()
        {
        }

        /// <summary>
        /// Exception
        /// </summary>
        /// <param name="message">message</param>
        public PrintLabelDoesNotExistException(string message) : base(message)
        {
        }

    }
}
