﻿//////////////////////////////////////////////////////////////
// <auto-generated>This code was generated by LLBLGen Pro 5.10.</auto-generated>
//////////////////////////////////////////////////////////////
// Code is generated on: 
// Code is generated using templates: SD.TemplateBindings.SharedTemplates
// Templates vendor: Solutions Design.
//////////////////////////////////////////////////////////////
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using ConTech.Data.HelperClasses;
using ConTech.Data.FactoryClasses;
using ConTech.Data.RelationClasses;

using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ConTech.Data.EntityClasses
{
	// __LLBLGENPRO_USER_CODE_REGION_START AdditionalNamespaces
	// __LLBLGENPRO_USER_CODE_REGION_END
	/// <summary>Entity class which represents the entity 'ProjectView'.<br/><br/></summary>
	[Serializable]
	public partial class ProjectViewEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		private EntityCollection<ViewLevelEntity> _viewLevels;
		private ProjectEntity _project;
		private UserEntity _user;
		private UserEntity _user1;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		private static ProjectViewEntityStaticMetaData _staticMetaData = new ProjectViewEntityStaticMetaData();
		private static ProjectViewRelations _relationsFactory = new ProjectViewRelations();

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name Project</summary>
			public static readonly string Project = "Project";
			/// <summary>Member name User</summary>
			public static readonly string User = "User";
			/// <summary>Member name User1</summary>
			public static readonly string User1 = "User1";
			/// <summary>Member name ViewLevels</summary>
			public static readonly string ViewLevels = "ViewLevels";
		}

		/// <summary>Static meta-data storage for navigator related information</summary>
		protected class ProjectViewEntityStaticMetaData : EntityStaticMetaDataBase
		{
			public ProjectViewEntityStaticMetaData()
			{
				SetEntityCoreInfo("ProjectViewEntity", InheritanceHierarchyType.None, false, (int)ConTech.Data.EntityType.ProjectViewEntity, typeof(ProjectViewEntity), typeof(ProjectViewEntityFactory), false);
				AddNavigatorMetaData<ProjectViewEntity, EntityCollection<ViewLevelEntity>>("ViewLevels", a => a._viewLevels, (a, b) => a._viewLevels = b, a => a.ViewLevels, () => new ProjectViewRelations().ViewLevelEntityUsingViewId, typeof(ViewLevelEntity), (int)ConTech.Data.EntityType.ViewLevelEntity);
				AddNavigatorMetaData<ProjectViewEntity, ProjectEntity>("Project", "ProjectViews", (a, b) => a._project = b, a => a._project, (a, b) => a.Project = b, ConTech.Data.RelationClasses.StaticProjectViewRelations.ProjectEntityUsingProjectIdStatic, ()=>new ProjectViewRelations().ProjectEntityUsingProjectId, null, new int[] { (int)ProjectViewFieldIndex.ProjectId }, null, true, (int)ConTech.Data.EntityType.ProjectEntity);
				AddNavigatorMetaData<ProjectViewEntity, UserEntity>("User", "ProjectViews", (a, b) => a._user = b, a => a._user, (a, b) => a.User = b, ConTech.Data.RelationClasses.StaticProjectViewRelations.UserEntityUsingCreatedByUserIdStatic, ()=>new ProjectViewRelations().UserEntityUsingCreatedByUserId, null, new int[] { (int)ProjectViewFieldIndex.CreatedByUserId }, null, true, (int)ConTech.Data.EntityType.UserEntity);
				AddNavigatorMetaData<ProjectViewEntity, UserEntity>("User1", "ProjectViews1", (a, b) => a._user1 = b, a => a._user1, (a, b) => a.User1 = b, ConTech.Data.RelationClasses.StaticProjectViewRelations.UserEntityUsingLastModifiedByUserIdStatic, ()=>new ProjectViewRelations().UserEntityUsingLastModifiedByUserId, null, new int[] { (int)ProjectViewFieldIndex.LastModifiedByUserId }, null, true, (int)ConTech.Data.EntityType.UserEntity);
			}
		}

		/// <summary>Static ctor</summary>
		static ProjectViewEntity()
		{
		}

		/// <summary> CTor</summary>
		public ProjectViewEntity()
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public ProjectViewEntity(IEntityFields2 fields)
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this ProjectViewEntity</param>
		public ProjectViewEntity(IValidator validator)
		{
			InitClassEmpty(validator, null);
		}

		/// <summary> CTor</summary>
		/// <param name="id">PK value for ProjectView which data should be fetched into this ProjectView object</param>
		public ProjectViewEntity(System.Int32 id) : this(id, null)
		{
		}

		/// <summary> CTor</summary>
		/// <param name="id">PK value for ProjectView which data should be fetched into this ProjectView object</param>
		/// <param name="validator">The custom validator object for this ProjectViewEntity</param>
		public ProjectViewEntity(System.Int32 id, IValidator validator)
		{
			InitClassEmpty(validator, null);
			this.Id = id;
		}

		/// <summary>Private CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ProjectViewEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			// __LLBLGENPRO_USER_CODE_REGION_START DeserializationConstructor
			// __LLBLGENPRO_USER_CODE_REGION_END
		}

		/// <summary>Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'ViewLevel' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoViewLevels() { return CreateRelationInfoForNavigator("ViewLevels"); }

		/// <summary>Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'Project' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoProject() { return CreateRelationInfoForNavigator("Project"); }

		/// <summary>Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'User' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoUser() { return CreateRelationInfoForNavigator("User"); }

		/// <summary>Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'User' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoUser1() { return CreateRelationInfoForNavigator("User1"); }
		
		/// <inheritdoc/>
		protected override EntityStaticMetaDataBase GetEntityStaticMetaData() {	return _staticMetaData; }

		/// <summary>Initializes the class members</summary>
		private void InitClassMembers()
		{
			PerformDependencyInjection();
			// __LLBLGENPRO_USER_CODE_REGION_START InitClassMembers
			// __LLBLGENPRO_USER_CODE_REGION_END
			OnInitClassMembersComplete();
		}

		/// <summary>Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this ProjectViewEntity</param>
		/// <param name="fields">Fields of this entity</param>
		private void InitClassEmpty(IValidator validator, IEntityFields2 fields)
		{
			OnInitializing();
			this.Fields = fields ?? CreateFields();
			this.Validator = validator;
			InitClassMembers();
			// __LLBLGENPRO_USER_CODE_REGION_START InitClassEmpty
			// __LLBLGENPRO_USER_CODE_REGION_END

			OnInitialized();
		}

		/// <summary>The relations object holding all relations of this entity with other entity classes.</summary>
		public static ProjectViewRelations Relations { get { return _relationsFactory; } }

		/// <summary>Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'ViewLevel' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathViewLevels { get { return _staticMetaData.GetPrefetchPathElement("ViewLevels", CommonEntityBase.CreateEntityCollection<ViewLevelEntity>()); } }

		/// <summary>Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Project' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathProject { get { return _staticMetaData.GetPrefetchPathElement("Project", CommonEntityBase.CreateEntityCollection<ProjectEntity>()); } }

		/// <summary>Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'User' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathUser { get { return _staticMetaData.GetPrefetchPathElement("User", CommonEntityBase.CreateEntityCollection<UserEntity>()); } }

		/// <summary>Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'User' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathUser1 { get { return _staticMetaData.GetPrefetchPathElement("User1", CommonEntityBase.CreateEntityCollection<UserEntity>()); } }

		/// <summary>The BackgroundPdf property of the Entity ProjectView<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProjectView"."BackgroundPdf".<br/>Table field type characteristics (type, precision, scale, length): VarBinary, 0, 0, 2147483647.<br/>Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.Byte[] BackgroundPdf
		{
			get { return (System.Byte[])GetValue((int)ProjectViewFieldIndex.BackgroundPdf, true); }
			set	{ SetValue((int)ProjectViewFieldIndex.BackgroundPdf, value); }
		}

		/// <summary>The CreatedByUserId property of the Entity ProjectView<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProjectView"."CreatedByUserId".<br/>Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0.<br/>Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> CreatedByUserId
		{
			get { return (Nullable<System.Int32>)GetValue((int)ProjectViewFieldIndex.CreatedByUserId, false); }
			set	{ SetValue((int)ProjectViewFieldIndex.CreatedByUserId, value); }
		}

		/// <summary>The DateCreatedUtc property of the Entity ProjectView<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProjectView"."DateCreatedUtc".<br/>Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0.<br/>Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.DateTime DateCreatedUtc
		{
			get { return (System.DateTime)GetValue((int)ProjectViewFieldIndex.DateCreatedUtc, true); }
			set	{ SetValue((int)ProjectViewFieldIndex.DateCreatedUtc, value); }
		}

		/// <summary>The Description property of the Entity ProjectView<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProjectView"."Description".<br/>Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 750.<br/>Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String Description
		{
			get { return (System.String)GetValue((int)ProjectViewFieldIndex.Description, true); }
			set	{ SetValue((int)ProjectViewFieldIndex.Description, value); }
		}

		/// <summary>The Id property of the Entity ProjectView<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProjectView"."Id".<br/>Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0.<br/>Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int32 Id
		{
			get { return (System.Int32)GetValue((int)ProjectViewFieldIndex.Id, true); }
			set { SetValue((int)ProjectViewFieldIndex.Id, value); }		}

		/// <summary>The LastModifiedByUserId property of the Entity ProjectView<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProjectView"."LastModifiedByUserId".<br/>Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0.<br/>Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> LastModifiedByUserId
		{
			get { return (Nullable<System.Int32>)GetValue((int)ProjectViewFieldIndex.LastModifiedByUserId, false); }
			set	{ SetValue((int)ProjectViewFieldIndex.LastModifiedByUserId, value); }
		}

		/// <summary>The LastModifiedUtc property of the Entity ProjectView<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProjectView"."LastModifiedUtc".<br/>Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0.<br/>Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.DateTime> LastModifiedUtc
		{
			get { return (Nullable<System.DateTime>)GetValue((int)ProjectViewFieldIndex.LastModifiedUtc, false); }
			set	{ SetValue((int)ProjectViewFieldIndex.LastModifiedUtc, value); }
		}

		/// <summary>The Name property of the Entity ProjectView<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProjectView"."Name".<br/>Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 200.<br/>Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Name
		{
			get { return (System.String)GetValue((int)ProjectViewFieldIndex.Name, true); }
			set	{ SetValue((int)ProjectViewFieldIndex.Name, value); }
		}

		/// <summary>The ObjectStatus property of the Entity ProjectView<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProjectView"."ObjectStatus".<br/>Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0.<br/>Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ObjectStatus
		{
			get { return (System.Int32)GetValue((int)ProjectViewFieldIndex.ObjectStatus, true); }
			set	{ SetValue((int)ProjectViewFieldIndex.ObjectStatus, value); }
		}

		/// <summary>The ProjectId property of the Entity ProjectView<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProjectView"."ProjectId".<br/>Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0.<br/>Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ProjectId
		{
			get { return (System.Int32)GetValue((int)ProjectViewFieldIndex.ProjectId, true); }
			set	{ SetValue((int)ProjectViewFieldIndex.ProjectId, value); }
		}

		/// <summary>Gets the EntityCollection with the related entities of type 'ViewLevelEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(ViewLevelEntity))]
		public virtual EntityCollection<ViewLevelEntity> ViewLevels { get { return GetOrCreateEntityCollection<ViewLevelEntity, ViewLevelEntityFactory>("ProjectView", true, false, ref _viewLevels); } }

		/// <summary>Gets / sets related entity of type 'ProjectEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(false)]
		public virtual ProjectEntity Project
		{
			get { return _project; }
			set { SetSingleRelatedEntityNavigator(value, "Project"); }
		}

		/// <summary>Gets / sets related entity of type 'UserEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(false)]
		public virtual UserEntity User
		{
			get { return _user; }
			set { SetSingleRelatedEntityNavigator(value, "User"); }
		}

		/// <summary>Gets / sets related entity of type 'UserEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(false)]
		public virtual UserEntity User1
		{
			get { return _user1; }
			set { SetSingleRelatedEntityNavigator(value, "User1"); }
		}

		// __LLBLGENPRO_USER_CODE_REGION_START CustomEntityCode
		// __LLBLGENPRO_USER_CODE_REGION_END

	}
}

namespace ConTech.Data
{
	public enum ProjectViewFieldIndex
	{
		///<summary>BackgroundPdf. </summary>
		BackgroundPdf,
		///<summary>CreatedByUserId. </summary>
		CreatedByUserId,
		///<summary>DateCreatedUtc. </summary>
		DateCreatedUtc,
		///<summary>Description. </summary>
		Description,
		///<summary>Id. </summary>
		Id,
		///<summary>LastModifiedByUserId. </summary>
		LastModifiedByUserId,
		///<summary>LastModifiedUtc. </summary>
		LastModifiedUtc,
		///<summary>Name. </summary>
		Name,
		///<summary>ObjectStatus. </summary>
		ObjectStatus,
		///<summary>ProjectId. </summary>
		ProjectId,
		/// <summary></summary>
		AmountOfFields
	}
}

namespace ConTech.Data.RelationClasses
{
	/// <summary>Implements the relations factory for the entity: ProjectView. </summary>
	public partial class ProjectViewRelations: RelationFactory
	{
		/// <summary>Returns a new IEntityRelation object, between ProjectViewEntity and ViewLevelEntity over the 1:n relation they have, using the relation between the fields: ProjectView.Id - ViewLevel.ViewId</summary>
		public virtual IEntityRelation ViewLevelEntityUsingViewId
		{
			get { return ModelInfoProviderSingleton.GetInstance().CreateRelation(RelationType.OneToMany, "ViewLevels", true, new[] { ProjectViewFields.Id, ViewLevelFields.ViewId }); }
		}

		/// <summary>Returns a new IEntityRelation object, between ProjectViewEntity and ProjectEntity over the m:1 relation they have, using the relation between the fields: ProjectView.ProjectId - Project.Id</summary>
		public virtual IEntityRelation ProjectEntityUsingProjectId
		{
			get	{ return ModelInfoProviderSingleton.GetInstance().CreateRelation(RelationType.ManyToOne, "Project", false, new[] { ProjectFields.Id, ProjectViewFields.ProjectId }); }
		}

		/// <summary>Returns a new IEntityRelation object, between ProjectViewEntity and UserEntity over the m:1 relation they have, using the relation between the fields: ProjectView.CreatedByUserId - User.Id</summary>
		public virtual IEntityRelation UserEntityUsingCreatedByUserId
		{
			get	{ return ModelInfoProviderSingleton.GetInstance().CreateRelation(RelationType.ManyToOne, "User", false, new[] { UserFields.Id, ProjectViewFields.CreatedByUserId }); }
		}

		/// <summary>Returns a new IEntityRelation object, between ProjectViewEntity and UserEntity over the m:1 relation they have, using the relation between the fields: ProjectView.LastModifiedByUserId - User.Id</summary>
		public virtual IEntityRelation UserEntityUsingLastModifiedByUserId
		{
			get	{ return ModelInfoProviderSingleton.GetInstance().CreateRelation(RelationType.ManyToOne, "User1", false, new[] { UserFields.Id, ProjectViewFields.LastModifiedByUserId }); }
		}

	}
	
	/// <summary>Static class which is used for providing relationship instances which are re-used internally for syncing</summary>
	internal static class StaticProjectViewRelations
	{
		internal static readonly IEntityRelation ViewLevelEntityUsingViewIdStatic = new ProjectViewRelations().ViewLevelEntityUsingViewId;
		internal static readonly IEntityRelation ProjectEntityUsingProjectIdStatic = new ProjectViewRelations().ProjectEntityUsingProjectId;
		internal static readonly IEntityRelation UserEntityUsingCreatedByUserIdStatic = new ProjectViewRelations().UserEntityUsingCreatedByUserId;
		internal static readonly IEntityRelation UserEntityUsingLastModifiedByUserIdStatic = new ProjectViewRelations().UserEntityUsingLastModifiedByUserId;

		/// <summary>CTor</summary>
		static StaticProjectViewRelations() { }
	}
}
