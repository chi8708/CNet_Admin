<template>
  <div>
    <el-table :data="tableData" border>
      <el-table-column prop="name" label="名称"></el-table-column>
      <el-table-column prop="age" label="年龄"></el-table-column>
      <el-table-column label="操作">
        <template slot-scope="scope">
          <el-button 
            size="mini"
            @click="handleEdit(scope.row)">编辑</el-button>
          <el-button
            size="mini"
            type="danger" 
            @click="handleDelete(scope.row)">删除</el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-dialog title="编辑" :visible.sync="dialogVisible">
      <!-- 编辑表单 -->
    </el-dialog>
  </div>
</template>

<script>
export default {
  data () {
    return {
      tableData: [],
      dialogVisible: false
    }
  },
  methods: {
    handleEdit (row) {
      this.dialogVisible = true
    },
    handleDelete (row) {
      this.$confirm('确认删除?')
        .then(() => {
          // 删除操作
          this.$message.success('删除成功')
        })
    }
  }
}
</script> 