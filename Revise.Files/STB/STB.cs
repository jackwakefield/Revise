#region License

/**
 * Copyright (C) 2012 Jack Wakefield
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

#endregion

using System.Collections.Generic;
using System.IO;
using Revise.Files.Exceptions;

namespace Revise.Files {
    /// <summary>
    /// Provides the ability to create, open and save STB files.
    /// </summary>
    public class STB {
        #region Constants

        private const string FILE_IDENTIFIER = "STB";

        private const byte DEFAULT_VERSION = 1;
        private const short DEFAULT_COLUMN_WIDTH = 50;
        private const int DEFAULT_ROW_HEIGHT = 17;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the file path of the loaded file.
        /// </summary>
        public string FilePath {
            get;
            private set;
        }

        /// <summary>
        /// Gets the number of cells.
        /// </summary>
        public int ColumnCount {
            get {
                return columns.Count;
            }
        }

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        public int RowCount {
            get {
                return rows.Count;
            }
        }

        /// <summary>
        /// Gets or sets the height of the row cell.
        /// </summary>
        /// <value>
        /// The height of the row cell.
        /// </value>
        public int RowHeight {
            get;
            set;
        }

        #endregion

        private STBColumn rootColumn;
        private List<STBColumn> columns;
        private List<STBRow> rows;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="STB"/> class.
        /// </summary>
        public STB() {
            rootColumn = new STBColumn();
            columns = new List<STBColumn>();
            rows = new List<STBRow>();
            
            Reset();
        }

        /// <summary>
        /// Gets the specified <see cref="Revise.Files.STBRow"/>.
        /// </summary>
        /// <exception cref="Revise.Exceptions.DataRowOutOfRangeException">Thrown when the specified row does not exist.</exception>
        public STBRow this[int row] {
            get {
                if (row < 0 || row > rows.Count - 1) {
                    throw new DataRowOutOfRangeException();
                }

                return rows[row];
            }
        }

        /// <summary>
        /// Loads an STB file at the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <exception cref="Revise.Exceptions.FileMissingException">Thrown when the specified file path does not exist.</exception>
        /// <exception cref="Revise.Exceptions.FileReadOnlyException">Thrown when the specified file is set to read-only.</exception>
        /// <exception cref="Revise.Exceptions.FileInUseException">Thrown when the specified file is in use by another process.</exception>
        /// <exception cref="Revise.Exceptions.FileIdentifierMismatchException">Thrown when the specified file has the incorrect file header expected.</exception>
        public void Load(string filePath) {
            FileInfo file = new FileInfo(filePath);

            if (!file.Exists) {
                throw new FileMissingException(filePath);
            }

            FileStream stream;

            try {
                stream = file.Open(FileMode.Open);
            } catch (IOException) {
                throw new FileInUseException(filePath);
            }

            FilePath = filePath;
            Reset();

            Load(stream);

            stream.Close();
        }

        /// <summary>
        /// Loads an STB file using the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <exception cref="Revise.Exceptions.FileIdentifierMismatchException">Thrown when the specified file has the incorrect file header expected.</exception>
        public void Load(Stream stream) {
            BinaryReader reader = new BinaryReader(stream);
            string identifier = reader.ReadString(3);

            if (string.Compare(identifier, FILE_IDENTIFIER, false) != 0) {
                throw new FileIdentifierMismatchException(FilePath, FILE_IDENTIFIER, identifier);
            }

            int version = (byte)(reader.ReadByte() - '0');

            reader.BaseStream.Seek(4, SeekOrigin.Current);
            int rowCount = reader.ReadInt32();
            int columnCount = reader.ReadInt32();
            RowHeight = reader.ReadInt32();

            SetRootColumnWidth(reader.ReadInt16());

            for (int i = 0; i < columnCount; i++) {
                columns.Add(new STBColumn());
                SetColumnWidth(i, reader.ReadInt16());
            }

            SetRootColumnName(reader.ReadShortString());

            for (int i = 0; i < columnCount; i++) {
                SetColumnName(i, reader.ReadShortString());
            }

            for (int i = 0; i < rowCount - 1; i++) {
                STBRow row = new STBRow(columnCount);
                row[0] = reader.ReadShortString();
                
                rows.Add(row);
            }

            for (int i = 0; i < rowCount - 1; i++) {
                STBRow row = rows[i];

                for (int j = 1; j < columnCount; j++) {
                    row[j] = reader.ReadShortString();
                }
            }
        }

        /// <summary>
        /// Saves the STB file at the previously loaded file path.
        /// </summary>
        /// <exception cref="Revise.Exceptions.FileNotLoadedException">Thrown when the load method has not been called before-hand.</exception>
        public void Save() {
            if (FilePath == null) {
                throw new FileNotLoadedException("The file must be loaded before saving without specifying a file path");
            }

            Save(FilePath);
        }

        /// <summary>
        /// Saves the STB file at the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <exception cref="Revise.Exceptions.FileReadOnlyException">Thrown when the specified file is set to read-only.</exception>
        /// <exception cref="Revise.Exceptions.FileInUseException">Thrown when the specified file is in use by another process.</exception>
        public void Save(string filePath) {
            FilePath = filePath;

            FileInfo file = new FileInfo(filePath);

            if (file.Exists && file.IsReadOnly) {
                throw new FileReadOnlyException(filePath);
            }

            FileStream stream;

            try {
                stream = file.Open(FileMode.Create);
            } catch (IOException) {
                throw new FileInUseException(filePath);
            }

            Save(stream);

            stream.Close();
        }

        /// <summary>
        /// Saves the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Save(Stream stream) {
            BinaryWriter writer = new BinaryWriter(stream);

            writer.WriteString(FILE_IDENTIFIER);
            writer.Write('1');
            writer.Write(0);
            writer.Write(RowCount + 1);
            writer.Write(ColumnCount);
            writer.Write(RowHeight);

            writer.Write(GetRootColumnWidth());

            columns.ForEach((column) => {
                writer.Write(column.Width);
            });

            writer.WriteShortString(GetRootColumnName());

            columns.ForEach((column) => {
                writer.WriteShortString(column.Name);
            });

            rows.ForEach((row) => {
                writer.WriteShortString(row[0]);
            });

            long position = stream.Position;

            rows.ForEach((row) => {
                for (int i = 1; i < ColumnCount; i++) {
                    writer.WriteShortString(row[i]);
                }
            });
            
            stream.Seek(FILE_IDENTIFIER.Length + 1, SeekOrigin.Begin);
            writer.Write((int)position);
        }

        /// <summary>
        /// Adds a column with the specified header name and column width.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <param name="width">The column width.</param>
        public void AddColumn(string name = "", short width = DEFAULT_COLUMN_WIDTH) {
            STBColumn column = new STBColumn();
            column.Name = name;
            column.Width = width;

            columns.Add(column);

            rows.ForEach((row) => {
                row.AddColumn();
            });
        }

        /// <summary>
        /// Removes the specified column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <exception cref="Revise.Exceptions.DataColumnOutOfRangeException">Thrown when the specified column is out of range.</exception>
        public void RemoveColumn(int column) {
            if (column < 0 || column > columns.Count - 1) {
                throw new DataColumnOutOfRangeException();
            }

            columns.RemoveAt(column);

            rows.ForEach((row) => {
                row.RemoveColumn(column);
            });
        }

        /// <summary>
        /// Sets the name of the root column.
        /// </summary>
        /// <param name="name">The column name.</param>
        public void SetRootColumnName(string name) {
            rootColumn.Name = name;
        }

        /// <summary>
        /// Gets the name of the root column.
        /// </summary>
        /// <returns>The column name.</returns>
        public string GetRootColumnName() {
            return rootColumn.Name;
        }

        /// <summary>
        /// Sets the name of the specified column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="name">The column name.</param>
        /// <exception cref="Revise.Exceptions.DataColumnOutOfRangeException">Thrown when the specified column is out of range.</exception>
        public void SetColumnName(int column, string name) {
            if (column < 0 || column > columns.Count - 1) {
                throw new DataColumnOutOfRangeException();
            }

            columns[column].Name = name;
        }

        /// <summary>
        /// Gets the name of the specified column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <exception cref="Revise.Exceptions.DataColumnOutOfRangeException">Thrown when the specified column is out of range.</exception>
        /// <returns>The column name.</returns>
        public string GetColumnName(int column) {
            if (column < 0 || column > columns.Count - 1) {
                throw new DataColumnOutOfRangeException();
            }

            return columns[column].Name;
        }

        /// <summary>
        /// Sets the width of the root column.
        /// </summary>
        /// <param name="width">The column width.</param>
        public void SetRootColumnWidth(short width) {
            rootColumn.Width = width;
        }

        /// <summary>
        /// Gets the width of the root column.
        /// </summary>
        /// <returns>The column width.</returns>
        public short GetRootColumnWidth() {
            return rootColumn.Width;
        }

        /// <summary>
        /// Sets the width of the specified column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="width">The column width.</param>
        /// <exception cref="Revise.Exceptions.DataColumnOutOfRangeException">Thrown when the specified column is out of range.</exception>
        public void SetColumnWidth(int column, short width) {
            if (column < 0 || column > columns.Count - 1) {
                throw new DataColumnOutOfRangeException();
            }

            columns[column].Width = width;
        }

        /// <summary>
        /// Gets the width of the specified column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <exception cref="Revise.Exceptions.DataColumnOutOfRangeException">Thrown when the specified column is out of range.</exception>
        /// <returns>The column width.</returns>
        public int GetColumnWidth(int column) {
            if (column < 0 || column > columns.Count - 1) {
                throw new DataColumnOutOfRangeException();
            }

            return columns[column].Width;
        }

        /// <summary>
        /// Adds a new row.
        /// </summary>
        /// <returns></returns>
        public STBRow AddRow() {
            STBRow row = new STBRow(ColumnCount);
            rows.Add(row);

            return row;
        }

        /// <summary>
        /// Removes the specified row.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <exception cref="Revise.Exceptions.DataRowOutOfRangeException">Thrown when the specified row is out of range.</exception>
        public void RemoveRow(int row) {
            if (row < 0 || row > rows.Count - 1) {
                throw new DataRowOutOfRangeException();
            }

            rows.RemoveAt(row);
        }

        /// <summary>
        /// Resets all properties to their default values.
        /// </summary>
        public void Reset() {
            rootColumn.Name = string.Empty;
            rootColumn.Width = DEFAULT_COLUMN_WIDTH;

            columns.Clear();
            rows.Clear();

            FilePath = null;
            RowHeight = DEFAULT_ROW_HEIGHT;
        }
    }
}