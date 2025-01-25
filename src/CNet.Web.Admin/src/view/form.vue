<template>
  <el-form :model="formData" :rules="rules" ref="form">
    <el-form-item label="用户名" prop="username">
      <el-input v-model="formData.username"></el-input>
    </el-form-item>
    
    <el-form-item label="选择" prop="select">
      <el-select v-model="formData.select">
        <el-option
          v-for="item in options"
          :key="item.value"
          :label="item.label"
          :value="item.value">
        </el-option>
      </el-select>
    </el-form-item>

    <el-form-item>
      <el-button type="primary" @click="handleSubmit">提交</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
export default {
  data () {
    return {
      formData: {
        username: '',
        select: ''
      },
      rules: {
        username: [
          { required: true, message: '请输入用户名', trigger: 'blur' }
        ]
      },
      options: []
    }
  },
  methods: {
    handleSubmit () {
      this.$refs.form.validate(valid => {
        if (valid) {
          // 提交表单
          this.$message.success('提交成功')
        }
      })
    }
  }
}
</script> 