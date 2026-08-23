targetScope = 'local'

extension utilities

resource assert 'Assert' = {
  name: 'This should fail!'
  condition: false
}
