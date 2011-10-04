// Copyright (c) 2008 Programação na Internet
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// MIT Licence (http://www.opensource.org/licenses/mit-license.php)
//
//
// Author:  Luís Falcão, Carlos Guedes, Nuno Datia
// Date:    quarta-feira, 19 de Novembro de 2008

//

using System.Collections.Generic;

namespace MS.OSM.Querys.DAL.Shared
{
    /// <summary>
    /// The IDalMapper is a base interface that defines the CRUD operations
    /// that can be applied to each DTO entity.
    /// <p>The available CRUD operations are:</p>
    /// <ul>
    ///   <li>Create</li>
    ///   <li>Get</li>
    ///   <li>Update</li>
    ///   <li>Delete</li>
    /// </ul>
    /// <p>By design, all operations returns a new instance (e.g: the 
    /// Create operation returns the inserted instance - with an ID and DAL relation)</p>
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="EntityList">The type of the entity list.</typeparam>
    /// <typeparam name="Key">The type of the entity key.</typeparam>
    public interface IDalMapper<TEntity, EntityList, Key>
        where TEntity : new()
        where EntityList : IEnumerable<TEntity>
    {
        /// <summary>
        /// Add an Entity to the Database.
        /// </summary>
        /// <param name="e">The entity to insert</param>
        /// <returns> the inserted entity</returns>
        TEntity Add(TEntity e);

        /// <summary>
        /// Get an entity from the Database.
        /// </summary>
        /// <param name="key">The entity's key</param>
        /// <returns>The entity with all the properties set</returns>
        TEntity Get(Key key);

        /// <summary>
        /// Get all Entitys from the Database.
        /// </summary>
        /// <returns></returns>
        EntityList GetAll();

        /// <summary>
        /// Update the Entity into the Database
        /// </summary>
        /// <param name="e">The entity to Update</param>
        /// <returns>the updated Entity or null if the operation did not succeed.</returns>
        TEntity Update(TEntity e);

        /// <summary>
        /// Delete the entity from Database.
        /// </summary>
        /// <param name="key">The entity to delete</param>
        /// <returns>The Entity that represents the deleted information from Database</returns>
        TEntity Delete(Key key);

    }
}