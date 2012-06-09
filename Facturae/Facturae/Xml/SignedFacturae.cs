﻿/* This file is part of Facturae.
 * 
 * Facturae is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * Facturae is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with Facturae.  If not, see <http://www.gnu.org/licenses/>.
 */

using System.Xml;
using nFacturae.Schemas;
using nFacturae.Xml;

namespace nFacturae
{
    public sealed class SignedFacturae
    {
        #region · Fields ·

        private XmlDocument signedDocument;

        #endregion

        #region · Constructors ·

        public SignedFacturae()
        {
        }

        public SignedFacturae(XmlDocument document)
        {
            this.signedDocument = document;
        }

        #endregion

        #region · Methods ·

        public SignedFacturae Load(string path)
        {
            this.signedDocument = new XmlDocument { PreserveWhitespace = true };
            this.signedDocument.Load(path);

            return this;
        }

        public SignedFacturae Load(XmlDocument document)
        {
            this.signedDocument = document;

            return this;
        }

        public SignedFacturae WriteToFile(string path)
        {
            this.signedDocument.Save(path);

            return this;
        }

        /// <summary>
        /// Verify the signature against an asymetric 
        /// algorithm and return the result.
        /// </summary>
        /// <param name="eInvoice"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        /// <remarks>http://social.msdn.microsoft.com/Forums/hu-HU/netfxbcl/thread/d6a4fe9f-7d2e-419c-ab19-9e57c75ba90f</remarks>
        public bool CheckSignature()
        {
            XaDESSignedXml      signedXml   = new XaDESSignedXml(this.signedDocument);
            XmlNamespaceManager nsmgr       = XsdSchemas.CreateXadesNamespaceManager(this.signedDocument);
            
            // Load the signature node.
            signedXml.LoadXml((XmlElement)this.signedDocument.SelectSingleNode("//ds:Signature", nsmgr));
            
            // Check the signature against the passed asymetric key
            // and return the result.
            return signedXml.CheckSignature();
        }

        #endregion
    }
}